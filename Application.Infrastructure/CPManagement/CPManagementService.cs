using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.CTO;
using LMPlatform.Data.Infrastructure;
using System;
using System.Data.Entity;
using System.Linq;
using Application.Core.Extensions;
using LMPlatform.Models.CP;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using System.Collections.Generic;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.ProjectManagement;

namespace Application.Infrastructure.CPManagement
{
    public class CPManagementService: ICPManagementService
    {
        public PagedList<CourseProjectData> GetProjects(int userId, GetPagedListParams parms)
        {
            var subjectId = int.Parse(parms.Filters["subjectId"]);
            var searchString = parms.Filters["searchString"];

            var query = Context.CourseProjects.AsNoTracking()
                .Include(x => x.Lecturer)
                .Include(x => x.AssignedCourseProjects.Select(asp => asp.Student.Group))
                .Include(x=>x.Subject);

            var user = Context.Users.Include(x => x.Student).Include(x => x.Lecturer).SingleOrDefault(x => x.Id == userId);

            if (user != null && user.Lecturer != null)
            {
                    query = query.Where(x => x.LecturerId == userId).Where(x => x.SubjectId == subjectId);
            }

            if (user != null && user.Student != null)
            {
                    query = query.Where(x => x.CourseProjectGroups.Any(dpg => dpg.GroupId == user.Student.GroupId)).Where(x => x.SubjectId == subjectId);
            }

            if (searchString.Length > 0)
            {
                var courseProjects = from cp in query
                                     let acp = cp.AssignedCourseProjects.FirstOrDefault()
                                     where acp.Student.LastName.Contains(searchString) || 
                                            cp.Theme.Contains(searchString) ||
                                            acp.Student.Group.Name.Contains(searchString)
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
            else
            {
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

            AuthorizationHelper.ValidateLecturerAccess(Context, projectData.LecturerId.Value);

            CourseProject project;
            if (projectData.Id.HasValue)
            {   
                project = Context.CourseProjects
                              .Include(x => x.CourseProjectGroups)
                              .Single(x => x.CourseProjectId == projectData.Id);
                if (Context.CourseProjects.Any(x => x.Theme == projectData.Theme && x.SubjectId == projectData.SubjectId && x.CourseProjectId != projectData.Id))
                {
                    throw new ApplicationException("Тема с таким названием уже есть!");
                }
            }
            else
            {
                if (Context.CourseProjects.Any(x => x.Theme == projectData.Theme && x.SubjectId == projectData.SubjectId))
                {
                    throw new ApplicationException("Тема с таким названием уже есть!");
                }
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
            var cp = Context.CourseProjects.FirstOrDefault(x => x.CourseProjectId == projectId);            
            cp.DateStart = isLecturer ? (DateTime?)DateTime.Now : null;
            Context.SaveChanges();

            if (cp.Subject.IsNeededCopyToBts)
            {
                CreateBtsProject(cp, studentId);
            }
        }

        public void DeleteAssignment(int userId, int id)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            var project = Context.AssignedCourseProjects.Single(x => x.CourseProjectId == id);
            var cp = Context.CourseProjects.FirstOrDefault(x => x.CourseProjectId == id);
            cp.DateStart = null;
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

            return Context.Students
                .Include(x => x.Group.CourseProjectGroups)
                .Where(x => x.Group.CourseProjectGroups.Any(dpg => dpg.CourseProjectId == courseProjectId))
                .Where(x => !x.AssignedCourseProjects.Any())
				.Where(x => x.Confirmed == null || x.Confirmed.Value)
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
            var query = Context.Students
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

        public PagedList<StudentData> GetGraduateStudentsForGroup(int userId, int groupId, int subjectId, GetPagedListParams parms, bool getBySecretaryForStudent = true)
        {
            var secretaryId = 0;
            if (parms.Filters.ContainsKey("secretaryId"))
            {
                int.TryParse(parms.Filters["secretaryId"], out secretaryId);
            }

            var searchString = "";

            if (parms.Filters.ContainsKey("searchString"))
            {
                searchString = parms.Filters["searchString"];
            }

            var isStudent = AuthorizationHelper.IsStudent(Context, userId);
            var isLecturer = AuthorizationHelper.IsLecturer(Context, userId);

            if (isStudent)
            {
                userId = Context.Users.Where(x => x.Id == userId)
                     .Select(x => x.Student.AssignedCourseProjects.FirstOrDefault().CourseProject.LecturerId)
                     .Single() ?? 0;
            }

                if (string.IsNullOrWhiteSpace(parms.SortExpression) || parms.SortExpression == "Id")
                {
                    parms.SortExpression = "Name";
                }
            var query = Context.Students
                .Where(x => x.GroupId == groupId)
                .Where(x => x.AssignedCourseProjects.Any(a => a.CourseProject.SubjectId == subjectId))
                .Where(x => x.AssignedCourseProjects.Any(a => a.CourseProject.LecturerId == userId));

            if (searchString.Length > 0)
            {
                return (from s in query
                        let lecturer = s.AssignedCourseProjects.FirstOrDefault().CourseProject.Lecturer
                        let cp = s.AssignedCourseProjects.FirstOrDefault()
                        where cp.CourseProject.Theme.Contains(searchString) || s.LastName.Contains(searchString)
                        select new StudentData
                        {
                            Id = s.Id,
                            Name = s.LastName + " " + s.FirstName + " " + s.MiddleName, //todo
                            Mark = s.AssignedCourseProjects.FirstOrDefault().Mark,
                            AssignedCourseProjectId = s.AssignedCourseProjects.FirstOrDefault().Id,
                            Lecturer = lecturer.LastName + " " + lecturer.FirstName + " " + lecturer.MiddleName, //todo
                            Group = s.AssignedCourseProjects.FirstOrDefault().CourseProject.Theme,
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
            else
            {
                return (from s in query
                        let lecturer = s.AssignedCourseProjects.FirstOrDefault().CourseProject.Lecturer
                        select new StudentData
                        {
                            Id = s.Id,
                            Name = s.LastName + " " + s.FirstName + " " + s.MiddleName, //todo
                            Mark = s.AssignedCourseProjects.FirstOrDefault().Mark,
                            AssignedCourseProjectId = s.AssignedCourseProjects.FirstOrDefault().Id,
                            Lecturer = lecturer.LastName + " " + lecturer.FirstName + " " + lecturer.MiddleName, //todo
                            Group = s.AssignedCourseProjects.FirstOrDefault().CourseProject.Theme,
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
                existingTemplate.Faculty = template.Faculty;
                existingTemplate.HeadCathedra = template.HeadCathedra;
                existingTemplate.Univer = template.Univer;
                existingTemplate.DateStart = template.DateStart;
                existingTemplate.DateEnd = template.DateEnd;
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
                RpzContent = dp.RpzContent,
                Faculty = dp.Faculty,
                HeadCathedra = dp.HeadCathedra,
                Univer = dp.Univer,
                DateEnd = dp.DateEnd,
                DateStart = dp.DateStart
                
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
            dp.HeadCathedra = taskSheet.HeadCathedra;
            dp.Faculty = taskSheet.Faculty;
            dp.Univer = taskSheet.Univer;
            dp.DateStart = taskSheet.DateStart;
            dp.DateEnd = taskSheet.DateEnd;

            Context.SaveChanges();
        }

        public SubjectData GetSubject(int id)
        {
            SubjectData sub = new SubjectData();
            Subject subject = new Subject();
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                subject = repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>(e => e.Id == id)
                    .Include(e => e.SubjectGroups));    
            }

            sub.Id = subject.Id;
            sub.Name = subject.Name;
            sub.ShortName = subject.ShortName;
            sub.IsNeededCopyToBts = subject.IsNeededCopyToBts;
            return sub;
        }

        public List<Correlation> GetGroups(int subjectId)
        {
            var groups = Context.Groups.Where(s => s.SubjectGroups.Any(d => d.SubjectId == subjectId && d.IsActiveOnCurrentGroup));


            return groups.OrderBy(x => x.Name).Select(x => new Correlation
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public List<NewsData> GetNewses(int userId, int subjectId)
        {
            var newses = Context.CourseProjectNewses.Where(x => x.SubjectId == subjectId).OrderByDescending(x=>x.EditDate);

            List<NewsData> list = new List<NewsData>();
            foreach (CourseProjectNews cp in newses)
            {
                NewsData n = new NewsData();
                n.Id = cp.Id;
                n.Title = cp.Title;
                n.Body = cp.Body;
                n.SubjectId = cp.SubjectId;
                n.DateCreate = cp.EditDate.ToShortDateString();
                n.Disabled = cp.Disabled;
                n.PathFile = cp.Attachments;
                n.Attachments = FilesManagementService.GetAttachments(cp.Attachments);
                list.Add(n);
            }
            return list;
        }

        public void SetSelectedGroupsToCourseProjects(int subjectId, List<int> groupIds)
        {
            var projects = Context.CourseProjects.Where(e => e.SubjectId == subjectId).Include(x => x.CourseProjectGroups);
            foreach (var project in projects)
            {
                foreach (int groupId in groupIds)
                {
                    CourseProjectGroup projectGroup = new CourseProjectGroup() {
                        CourseProjectId = project.CourseProjectId,
                        GroupId = groupId
                    };
                    project.CourseProjectGroups.Add(projectGroup);
                }
            }

            Context.SaveChanges();
        }

        private void CreateBtsProject(CourseProject courseProject, int developerId)
        {            
            int lecturerId = (int)courseProject.LecturerId;
            var project = new Project();
            project.CreatorId = lecturerId;
            project.Title = courseProject.Theme;
            project.DateOfChange = DateTime.Now;
            ProjectManagementService.SaveProject(project);

            ProjectManagementService.AssingRole(new ProjectUser
            {
                UserId = lecturerId,
                ProjectId = project.Id,
                ProjectRoleId = ProjectRole.Leader
            });

            ProjectManagementService.AssingRole(new ProjectUser
            {
                UserId = developerId,
                ProjectId = project.Id,
                ProjectRoleId = ProjectRole.Developer
            });
        }

        private string GetGuidFileName()
        {
            return string.Format("P{0}", Guid.NewGuid().ToString("N").ToUpper());
        }

        public CourseProjectNews SaveNews(CourseProjectNews news, IList<Attachment> attachments, Int32 userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                if (!string.IsNullOrEmpty(news.Attachments))
                {
                    var deleteFiles =
                        repositoriesContainer.AttachmentRepository.GetAll(
                            new Query<Attachment>(e => e.PathName == news.Attachments)).ToList().Where(e => attachments.All(x => x.Id != e.Id)).ToList();

                    foreach (var attachment in deleteFiles)
                    {
                        FilesManagementService.DeleteFileAttachment(attachment);
                    }
                }
                else
                {
                    news.Attachments = GetGuidFileName();
                }

                FilesManagementService.SaveFiles(attachments.Where(e => e.Id == 0), news.Attachments);

                foreach (var attachment in attachments)
                {
                    if (attachment.Id == 0)
                    {
                        attachment.PathName = news.Attachments;
                        repositoriesContainer.AttachmentRepository.Save(attachment);
                    }
                }
            }

            var cpEditNews = Context.CourseProjectNewses.FirstOrDefault(x => x.Id == news.Id && x.SubjectId == news.SubjectId);
            if (cpEditNews != null)
            {
                CourseProjectNews cpnews = new CourseProjectNews();
                cpEditNews.Body = news.Body;
                cpEditNews.Disabled = news.Disabled;
                cpEditNews.EditDate = news.EditDate;
                cpEditNews.SubjectId = news.SubjectId;
                cpEditNews.Attachments = news.Attachments;
                cpEditNews.Title = news.Title;
            }
            else {
                Context.CourseProjectNewses.Add(news);
            }
            Context.SaveChanges();

            return news;
        }

        public void DeleteNews(CourseProjectNews news)
        {
            var cpnews = Context.CourseProjectNewses.Single(x => x.Id == news.Id && x.SubjectId==news.SubjectId);
            Context.CourseProjectNewses.Remove(cpnews);
            Context.SaveChanges();
        }

        public void DisableNews(int subjectId, bool disable)
        {
           var models = Context.CourseProjectNewses.Where(e => e.SubjectId == subjectId);

           foreach (var cpNews in models)
           {
               cpNews.Disabled = disable;
           }

           Context.SaveChanges();
        }

        public CourseProjectNews GetNews(int id)
        {
            return Context.CourseProjectNewses.Single(x => x.Id == id);
        }

        public void DeleteUserFromAcpProject(int id, int projectId)
        {
            var acp = Context.AssignedCourseProjects.Single(e => e.CourseProjectId == projectId && e.StudentId == id);
            Context.AssignedCourseProjects.Remove(acp);
            Context.SaveChanges();
        }

        public void DeletePercenageAndVisitStatsForUser(int id)
        {
            var cpPR = Context.CoursePercentagesResults.Where(e => e.StudentId == id);
            foreach(var cp in cpPR)
            {
                Context.CoursePercentagesResults.Remove(cp);
            }

            var cpVS = Context.CourseProjectConsultationMarks.Where(e => e.StudentId == id);
            foreach (var cp in cpVS)
            {
                Context.CourseProjectConsultationMarks.Remove(cp);
            }
            Context.SaveChanges();
        }

        private readonly LazyDependency<ICpContext> context = new LazyDependency<ICpContext>();

        private ICpContext Context
        {
            get { return context.Value; }
        }

        private readonly LazyDependency<IFilesManagementService> _filesManagementService =
            new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get { return _filesManagementService.Value; }
        }

        private readonly LazyDependency<IProjectManagementService> _projectManagementService =
            new LazyDependency<IProjectManagementService>();

        public IProjectManagementService ProjectManagementService
        {
            get { return _projectManagementService.Value; }
        }
    }
}
