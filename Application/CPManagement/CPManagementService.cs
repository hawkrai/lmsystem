using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.CTO;
using LMPlatform.Data.Infrastructure;
using System;
using System.Data.Entity;
using System.Linq;
using Application.Core.Extensions;
using LMPlatform.Models.CP;


namespace Application.Infrastructure.CPManagement
{
    public class CPManagementService: ICPManagementService
    {
        public PagedList<CourseProjectData> GetProjects(int userId, GetPagedListParams parms)
        {
            var subjectId = int.Parse(parms.Filters["subjectId"]);

            var query = Context.CourseProjects.AsNoTracking()
                .Include(x => x.Lecturer)
                .Include(x => x.AssignedCourseProjects.Select(asp => asp.Student.Group))
                .Include(x=>x.Subject);

            var user = Context.Users.Include(x => x.Student).Include(x => x.Lecturer).SingleOrDefault(x => x.Id == userId);

            if (user != null && user.Lecturer != null)
            {
                query = query.Where(x => x.LecturerId == userId).Where(x=>x.SubjectId==subjectId);
            }

            if (user != null && user.Student != null)
            {
                query = query.Where(x => x.CourseProjectGroups.Any(dpg => dpg.GroupId == user.Student.GroupId)).Where(x => x.SubjectId == subjectId);
            }

            var courseProjects = from cp in query
                                 let acp = cp.AssignedCourseProjects.FirstOrDefault()
                                 select new CourseProjectData
                                 {
                                     Id = cp.CourseProjectId,
                                     Theme = cp.Theme,
                                     Lecturer = cp.Lecturer != null ? cp.Lecturer.LastName + " " + cp.Lecturer.FirstName + " " + cp.Lecturer.MiddleName : null,
                                     Student = acp.Student != null ? acp.Student.LastName + " " + acp.Student.FirstName + " " + acp.Student.MiddleName : null,
                                     StudentId = acp.StudentId,
                                     Group = acp.Student.Group.Name,
                                     ApproveDate = acp.ApproveDate
                                 };

            return courseProjects.ApplyPaging(parms);
            
        }

        public CourseProjectData GetProject(int id)
        {
            var cp = Context.CourseProjects
                .AsNoTracking()
                .Include(x => x.CourseProjectGroups)
                .Single(x => x.CourseProjectId == id);
            return new CourseProjectData
            {
                Id = cp.CourseProjectId,
                Theme = cp.Theme,
                SelectedGroupsIds = cp.CourseProjectGroups.Select(x => x.GroupId)
            };
        }

        public void SaveProject(CourseProjectData projectData)
        {
            if (!projectData.LecturerId.HasValue)
            {
                throw new ApplicationException("LecturerId cant be empty!");
            }

            if (Context.CourseProjects.Any(x => x.Theme == projectData.Theme && x.SubjectId == projectData.SubjectId))
            {
                throw new ApplicationException("Тема с таким названием уже есть!");
            }

            AuthorizationHelper.ValidateLecturerAccess(Context, projectData.LecturerId.Value);

            CourseProject project;
            if (projectData.Id.HasValue)
            {
                project = Context.CourseProjects
                              .Include(x => x.CourseProjectGroups)
                              .Single(x => x.CourseProjectId == projectData.Id);
            }
            else
            {
                project = new CourseProject();
                Context.CourseProjects.Add(project);
            }

            var currentGroups = project.CourseProjectGroups.ToList();
            var newGroups = projectData.SelectedGroupsIds.Select(x => new CourseProjectGroup { GroupId = x, CourseProjectId = project.CourseProjectId }).ToList();

            var groupsToAdd = newGroups.Except(currentGroups, grp => grp.GroupId);
            var groupsToDelete = currentGroups.Except(newGroups, grp => grp.GroupId);

            foreach (var projectGroup in groupsToAdd)
            {
                project.CourseProjectGroups.Add(projectGroup);
            }

            foreach (var projectGroup in groupsToDelete)
            {
                Context.CourseProjectGroups.Remove(projectGroup);
            }

            project.LecturerId = projectData.LecturerId.Value;
            project.Theme = projectData.Theme;
            project.SubjectId = projectData.SubjectId.Value;
            Context.SaveChanges();
        }

        public void DeleteProject(int userId, int id)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            var project = Context.CourseProjects.Single(x => x.CourseProjectId == id);
            Context.CourseProjects.Remove(project);
            Context.SaveChanges();
        }

        public void AssignProject(int userId, int projectId, int studentId)
        {
            var isLecturer = AuthorizationHelper.IsLecturer(Context, userId);
            var isStudent = AuthorizationHelper.IsStudent(Context, userId);
            studentId = isStudent ? userId : studentId;

            var assignment = Context.AssignedCourseProjects.FirstOrDefault(x => x.CourseProjectId == projectId);

            if ((isLecturer && assignment != null && assignment.ApproveDate.HasValue)
                || (isStudent && assignment != null))
            {
                throw new ApplicationException("The selected Diplom Project has already been assigned!");
            }

            var studentAssignments = Context.AssignedCourseProjects.Where(x => x.StudentId == studentId);

            if (isStudent && studentAssignments.Any(x => x.ApproveDate.HasValue))
            {
                throw new ApplicationException("You already have an assigned project!");
            }

            foreach (var studentAssignment in studentAssignments)
            {
                Context.AssignedCourseProjects.Remove(studentAssignment);
            }

            if (assignment == null)
            {
                assignment = new AssignedCourseProject
                {
                    CourseProjectId = projectId
                };
                Context.AssignedCourseProjects.Add(assignment);
            }

            assignment.StudentId = studentId == 0 ? assignment.StudentId : studentId;
            assignment.ApproveDate = isLecturer ? (DateTime?)DateTime.Now : null;
            Context.SaveChanges();
        }

        public void DeleteAssignment(int userId, int id)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            var project = Context.AssignedCourseProjects.Single(x => x.CourseProjectId == id);
            Context.AssignedCourseProjects.Remove(project);
            Context.SaveChanges();
        }

        public PagedList<StudentData> GetStudentsByCourseProjectId(GetPagedListParams parms)
        {
            if (!parms.Filters.ContainsKey("courseProjectId"))
            {
                throw new ApplicationException("coursePorjectId can't be empty!");
            }

            parms.SortExpression = "Group, Name";

            var courseProjectId = int.Parse(parms.Filters["courseProjectId"]);

            return Context.GetGraduateStudents()
                .Include(x => x.Group.CourseProjectGroups)
                .Where(x => x.Group.CourseProjectGroups.Any(dpg => dpg.CourseProjectId == courseProjectId))
                .Where(x => !x.AssignedCourseProjects.Any())
                .Select(s => new StudentData
                {
                    Id = s.Id,
                    Name = s.LastName + " " + s.FirstName + " " + s.MiddleName, //todo
                    Group = s.Group.Name
                }).ApplyPaging(parms);
        }

        public PagedList<StudentData> GetGraduateStudentsForUser(int userId, int subjectId, GetPagedListParams parms, bool getBySecretaryForStudent = true)
        {
            var secretaryId = 0;
            if (parms.Filters.ContainsKey("secretaryId"))
            {
                int.TryParse(parms.Filters["secretaryId"], out secretaryId);
            }

            var isStudent = AuthorizationHelper.IsStudent(Context, userId);
            var isLecturer = AuthorizationHelper.IsLecturer(Context, userId);
            var isLecturerSecretary = isLecturer && Context.Lecturers.Single(x => x.Id == userId).IsSecretary;
            secretaryId = isLecturerSecretary ? userId : secretaryId;
            if (isStudent)
            {
                if (getBySecretaryForStudent)
                {
                    secretaryId = Context.Users.Where(x => x.Id == userId).Select(x => x.Student.Group.SecretaryId).Single() ?? 0;
                }
                else
                {
                    userId = Context.Users.Where(x => x.Id == userId)
                            .Select(x => x.Student.AssignedCourseProjects.FirstOrDefault().CourseProject.LecturerId)
                            .Single() ?? 0;
                }
            }

            if (string.IsNullOrWhiteSpace(parms.SortExpression) || parms.SortExpression == "Id")
            {
                parms.SortExpression = "Name";
            }
            var query = Context.GetGraduateStudents()
                .Where(x => isLecturerSecretary || (isStudent && getBySecretaryForStudent) || x.AssignedCourseProjects.Any(asd => asd.CourseProject.LecturerId == userId && asd.CourseProject.SubjectId == subjectId))
                .Where(x => secretaryId == 0 || x.Group.SecretaryId == secretaryId)
                .Where(x => x.AssignedCourseProjects.Any(a => a.CourseProject.SubjectId == subjectId));
            return (from s in query
                    let lecturer = s.AssignedCourseProjects.FirstOrDefault().CourseProject.Lecturer
                    select new StudentData
                    {
                        Id = s.Id,
                        Name = s.LastName + " " + s.FirstName + " " + s.MiddleName, //todo
                        Mark = s.AssignedCourseProjects.FirstOrDefault().Mark,
                        AssignedCourseProjectId = s.AssignedCourseProjects.FirstOrDefault().Id,
                        Lecturer = lecturer.LastName + " " + lecturer.FirstName + " " + lecturer.MiddleName, //todo
                        Group = s.Group.Name,
                        PercentageResults = s.CoursePercentagesResults.Select(pr => new PercentageResultData
                        {
                            Id = pr.Id,
                            PercentageGraphId = pr.CoursePercentagesGraphId,
                            StudentId = pr.StudentId,
                            Mark = pr.Mark,
                            Comment = pr.Comments
                        }),
                        CourseProjectConsultationMarks = s.CourseProjectConsultationMarks.Select(cm => new CourseProjectConsultationMarkData
                        {
                            Id = cm.Id,
                            ConsultationDateId = cm.ConsultationDateId,
                            StudentId = cm.StudentId,
                            Mark = cm.Mark,
                            Comments = cm.Comments
                        })
                    }).ApplyPaging(parms);
        }

        public void SetStudentDiplomMark(int lecturerId, int assignedProjectId, int mark)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, lecturerId);
            Context.AssignedCourseProjects.Single(x => x.Id == assignedProjectId).Mark = mark;
            Context.SaveChanges();
        }

        public CourseProjectTaskSheetTemplate GetTaskSheetTemplate(int id)
        {
            return Context.CourseProjectTaskSheetTemplates.Single(x => x.Id == id);
        }

        public void SaveTaskSheetTemplate(CourseProjectTaskSheetTemplate template)
        {
            var existingTemplate = Context.CourseProjectTaskSheetTemplates.FirstOrDefault(x => x.Name.Trim() == template.Name.Trim())
                ?? Context.CourseProjectTaskSheetTemplates.SingleOrDefault(x => x.Id == template.Id);
            if (existingTemplate != null)
            {
                existingTemplate.Name = template.Name;
                existingTemplate.InputData = template.InputData;
                existingTemplate.Consultants = template.Consultants;
                existingTemplate.DrawMaterials = template.DrawMaterials;
                existingTemplate.RpzContent = template.RpzContent;
                existingTemplate.LecturerId = template.LecturerId;
            }
            else
            {
                Context.CourseProjectTaskSheetTemplates.Add(template);
            }

            Context.SaveChanges();
        }

        public TaskSheetData GetTaskSheet(int courseProjectId)
        {
            var dp = Context.CourseProjects.Single(x => x.CourseProjectId == courseProjectId);
            return new TaskSheetData
            {
                InputData = dp.InputData,
                Consultants = dp.Consultants,
                CourseProjectId = dp.CourseProjectId,
                DrawMaterials = dp.DrawMaterials,
                RpzContent = dp.RpzContent
            };
        }

        public void SaveTaskSheet(int userId, TaskSheetData taskSheet)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            var dp = Context.CourseProjects.Single(x => x.CourseProjectId == taskSheet.CourseProjectId);

            dp.InputData = taskSheet.InputData;
            dp.RpzContent = taskSheet.RpzContent;
            dp.DrawMaterials = taskSheet.DrawMaterials;
            dp.Consultants = taskSheet.Consultants;

            Context.SaveChanges();
        }

        private readonly LazyDependency<ICpContext> context = new LazyDependency<ICpContext>();

        private ICpContext Context
        {
            get { return context.Value; }
        }
            
    }
}
