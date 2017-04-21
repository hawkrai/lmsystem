using System;
using System.Data.Entity;
using System.Linq;
using Application.Core;
using Application.Core.Data;
using Application.Core.Extensions;
using Application.Infrastructure.DTO;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models;
using LMPlatform.Models.DP;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using Application.Infrastructure.FilesManagement;
using LMPlatform.Data.Repositories;

namespace Application.Infrastructure.DPManagement
{
    public class DpManagementService : IDpManagementService
    {
        public PagedList<DiplomProjectData> GetProjects(int userId, GetPagedListParams parms)
        {
            var searchString = parms.Filters["searchString"];
            var query = Context.DiplomProjects.AsNoTracking()
                .Include(x => x.Lecturer)
                .Include(x => x.AssignedDiplomProjects.Select(asp => asp.Student.Group));

            var user = Context.Users.Include(x => x.Student).Include(x => x.Lecturer).SingleOrDefault(x => x.Id == userId);

            if (user != null && user.Lecturer != null && !user.Lecturer.IsSecretary)
            {
                query = query.Where(x => x.LecturerId == userId);
            }

            if (user != null && user.Lecturer != null && user.Lecturer.IsSecretary)
            {
                query = query.Where(x => x.AssignedDiplomProjects.Any());
            }

            if (user != null && user.Student != null)
            {
                query = query.Where(x => x.DiplomProjectGroups.Any(dpg => dpg.GroupId == user.Student.GroupId));
            }

            if (searchString.Length > 0)
            {
                var diplomProjects = from dp in query
                                     let adp = dp.AssignedDiplomProjects.FirstOrDefault()
                                     where adp.Student.LastName.Contains(searchString) ||
                                         dp.Theme.Contains(searchString) ||
                                        adp.Student.Group.Name.Contains(searchString)
                                     select new DiplomProjectData
                                     {
                                         Id = dp.DiplomProjectId,
                                         Theme = dp.Theme,
                                         Lecturer = dp.Lecturer != null ? dp.Lecturer.LastName + " " + dp.Lecturer.FirstName + " " + dp.Lecturer.MiddleName : null,
                                         Student = adp.Student != null ? adp.Student.LastName + " " + adp.Student.FirstName + " " + adp.Student.MiddleName : null,
                                         StudentId = adp.StudentId,
                                         Group = adp.Student.Group.Name,
                                         ApproveDate = adp.ApproveDate
                                     };

                return diplomProjects.ApplyPaging(parms);
            }
            else
            {
                var diplomProjects = from dp in query
                                     let adp = dp.AssignedDiplomProjects.FirstOrDefault()
                                     select new DiplomProjectData
                                     {
                                         Id = dp.DiplomProjectId,
                                         Theme = dp.Theme,
                                         Lecturer = dp.Lecturer != null ? dp.Lecturer.LastName + " " + dp.Lecturer.FirstName + " " + dp.Lecturer.MiddleName : null,
                                         Student = adp.Student != null ? adp.Student.LastName + " " + adp.Student.FirstName + " " + adp.Student.MiddleName : null,
                                         StudentId = adp.StudentId,
                                         Group = adp.Student.Group.Name,
                                         ApproveDate = adp.ApproveDate
                                     };

                return diplomProjects.ApplyPaging(parms);
            }
        }

        public DiplomProjectData GetProject(int id)
        {
            var dp = Context.DiplomProjects
                .AsNoTracking()
                .Include(x => x.DiplomProjectGroups)
                .Single(x => x.DiplomProjectId == id);
            return new DiplomProjectData
                {
                    Id = dp.DiplomProjectId,
                    Theme = dp.Theme,
                    SelectedGroupsIds = dp.DiplomProjectGroups.Select(x => x.GroupId)
                };
        }

        public void SaveProject(DiplomProjectData projectData)
        {
            if (!projectData.LecturerId.HasValue)
            {
                throw new ApplicationException("LecturerId cant be empty!");
            }

            if (Context.DiplomProjects.Any(x => x.Theme == projectData.Theme))
            {
                throw new ApplicationException("Тема с таким названием уже есть!");
            }

            AuthorizationHelper.ValidateLecturerAccess(Context, projectData.LecturerId.Value);

            DiplomProject project;
            if (projectData.Id.HasValue)
            {
                project = Context.DiplomProjects
                              .Include(x => x.DiplomProjectGroups)
                              .Single(x => x.DiplomProjectId == projectData.Id);
            }
            else
            {
                project = new DiplomProject();
                Context.DiplomProjects.Add(project);
            }

            var currentGroups = project.DiplomProjectGroups.ToList();
            var newGroups = projectData.SelectedGroupsIds.Select(x => new DiplomProjectGroup { GroupId = x, DiplomProjectId = project.DiplomProjectId }).ToList();

            var groupsToAdd = newGroups.Except(currentGroups, grp => grp.GroupId);
            var groupsToDelete = currentGroups.Except(newGroups, grp => grp.GroupId);

            foreach (var projectGroup in groupsToAdd)
            {
                project.DiplomProjectGroups.Add(projectGroup);
            }

            foreach (var projectGroup in groupsToDelete)
            {
                Context.DiplomProjectGroups.Remove(projectGroup);
            }

            project.LecturerId = projectData.LecturerId.Value;
            project.Theme = projectData.Theme;
            Context.SaveChanges();
        }

        public TaskSheetData GetTaskSheet(int diplomProjectId)
        {
            var dp = Context.DiplomProjects.Single(x => x.DiplomProjectId == diplomProjectId);
            return new TaskSheetData
            {
                InputData = dp.InputData,
                Consultants = dp.Consultants,
                DiplomProjectId = dp.DiplomProjectId,
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

            var dp = Context.DiplomProjects.Single(x => x.DiplomProjectId == taskSheet.DiplomProjectId);

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

        public void DeleteProject(int userId, int id)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            var project = Context.DiplomProjects.Single(x => x.DiplomProjectId == id);
            Context.DiplomProjects.Remove(project);
            Context.SaveChanges();
        }

        public void AssignProject(int userId, int projectId, int studentId)
        {
            var isLecturer = AuthorizationHelper.IsLecturer(Context, userId);
            var isStudent = AuthorizationHelper.IsStudent(Context, userId);
            studentId = isStudent ? userId : studentId;

            var assignment = Context.AssignedDiplomProjects.FirstOrDefault(x => x.DiplomProjectId == projectId);

            if ((isLecturer && assignment != null && assignment.ApproveDate.HasValue)
                || (isStudent && assignment != null))
            {
                throw new ApplicationException("The selected Diplom Project has already been assigned!");
            }

            var studentAssignments = Context.AssignedDiplomProjects.Where(x => x.StudentId == studentId);

            if (isStudent && studentAssignments.Any(x => x.ApproveDate.HasValue))
            {
                throw new ApplicationException("You already have an assigned project!");
            }

            foreach (var studentAssignment in studentAssignments)
            {
                Context.AssignedDiplomProjects.Remove(studentAssignment);
            }

            if (assignment == null)
            {
                assignment = new AssignedDiplomProject
                {
                    DiplomProjectId = projectId
                };
                Context.AssignedDiplomProjects.Add(assignment);
            }

            assignment.StudentId = studentId == 0 ? assignment.StudentId : studentId;
            assignment.ApproveDate = isLecturer ? (DateTime?)DateTime.Now : null;
            var dp = Context.DiplomProjects.FirstOrDefault(x => x.DiplomProjectId == projectId);
            dp.DateStart = isLecturer ? (DateTime?)DateTime.Now : null;
            Context.SaveChanges();
        }

        public void SetStudentDiplomMark(int lecturerId, int assignedProjectId, int mark)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, lecturerId);
            Context.AssignedDiplomProjects.Single(x => x.Id == assignedProjectId).Mark = mark;
            Context.SaveChanges();
        }

        public void DeleteAssignment(int userId, int id)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            var project = Context.AssignedDiplomProjects.Single(x => x.DiplomProjectId == id);
            Context.AssignedDiplomProjects.Remove(project);
            Context.SaveChanges();
        }

        public PagedList<StudentData> GetStudentsByDiplomProjectId(GetPagedListParams parms)
        {
            if (!parms.Filters.ContainsKey("diplomProjectId"))
            {
                throw new ApplicationException("diplomPorjectId can't be empty!");
            }

            parms.SortExpression = "Group, Name";

            var diplomProjectId = int.Parse(parms.Filters["diplomProjectId"]);

            return Context.GetGraduateStudents()
                .Include(x => x.Group.DiplomProjectGroups)
                .Where(x => x.Group.DiplomProjectGroups.Any(dpg => dpg.DiplomProjectId == diplomProjectId))
                .Where(x => !x.AssignedDiplomProjects.Any())
				.Where(x => x.Confirmed == null || x.Confirmed.Value)
                .Select(s => new StudentData
                {
                    Id = s.Id,
                    Name = s.LastName + " " + s.FirstName + " " + s.MiddleName, //todo
                    Group = s.Group.Name
                }).ApplyPaging(parms);
        }

        public PagedList<StudentData> GetGraduateStudentsForUser(int userId, GetPagedListParams parms, bool getBySecretaryForStudent = true)
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
                            .Select(x => x.Student.AssignedDiplomProjects.FirstOrDefault().DiplomProject.LecturerId)
                            .Single() ?? 0;
                }
            }

            if (string.IsNullOrWhiteSpace(parms.SortExpression) || parms.SortExpression == "Id")
            {
                parms.SortExpression = "Name";
            }
            var query = Context.GetGraduateStudents()
                .Where(x => isLecturerSecretary || (isStudent && getBySecretaryForStudent) || x.AssignedDiplomProjects.Any(asd => asd.DiplomProject.LecturerId == userId))
                .Where(x => secretaryId == 0 || x.Group.SecretaryId == secretaryId);
            return (from s in query
                    let lecturer = s.AssignedDiplomProjects.FirstOrDefault().DiplomProject.Lecturer
                    select new StudentData
                {
                    Id = s.Id,
                    Name = s.LastName + " " + s.FirstName + " " + s.MiddleName, //todo
                    Mark = s.AssignedDiplomProjects.FirstOrDefault().Mark,
                    AssignedDiplomProjectId = s.AssignedDiplomProjects.FirstOrDefault().Id,
                    Lecturer = lecturer.LastName + " " + lecturer.FirstName + " " + lecturer.MiddleName, //todo
                    Group = s.Group.Name,
                    PercentageResults = s.PercentagesResults.Select(pr => new PercentageResultData
                    {
                        Id = pr.Id,
                        PercentageGraphId = pr.DiplomPercentagesGraphId,
                        StudentId = pr.StudentId,
                        Mark = pr.Mark,
                        Comment = pr.Comments
                    }),
                    DipomProjectConsultationMarks = s.DiplomProjectConsultationMarks.Select(cm => new DipomProjectConsultationMarkData
                    {
                        Id = cm.Id,
                        ConsultationDateId = cm.ConsultationDateId,
                        StudentId = cm.StudentId,
                        Mark = cm.Mark,
                        Comments = cm.Comments
                    })
                }).ApplyPaging(parms);
        }

        public bool ShowDpSectionForUser(int userId)
        {
            if (AuthorizationHelper.IsStudent(Context, userId))
            {
                return AuthorizationHelper.IsGraduateStudent(Context, userId);
            }

            var lecturer = Context.Lecturers.Single(x => x.Id == userId);
            return lecturer.IsLecturerHasGraduateStudents || lecturer.IsSecretary;
        }

        public DiplomProjectTaskSheetTemplate GetTaskSheetTemplate(int id)
        {
            return Context.DiplomProjectTaskSheetTemplates.Single(x => x.Id == id);
        }

        public void SaveTaskSheetTemplate(DiplomProjectTaskSheetTemplate template)
        {
            var existingTemplate = Context.DiplomProjectTaskSheetTemplates.FirstOrDefault(x => x.Name.Trim() == template.Name.Trim())
                ?? Context.DiplomProjectTaskSheetTemplates.SingleOrDefault(x => x.Id == template.Id);
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
                Context.DiplomProjectTaskSheetTemplates.Add(template);
            }

            Context.SaveChanges();
        }

        public List<NewsData> GetNewses(int lecturerId)
        {
            List<NewsData> list = new List<NewsData>();
            var user = Context.Users.Any(x=>x.Lecturer.Id == lecturerId);
            if (user) {
                var newses = Context.DiplomProjectNewses.Where(x => x.LecturerId == lecturerId).OrderByDescending(x => x.EditDate);
                foreach (DiplomProjectNews cp in newses)
                {
                    NewsData n = new NewsData();
                    n.Id = cp.Id;
                    n.Title = cp.Title;
                    n.Body = cp.Body;
                    n.LecturerId = cp.LecturerId;
                    n.DateCreate = cp.EditDate.ToShortDateString();
                    n.Disabled = cp.Disabled;
                    n.PathFile = cp.Attachments;
                    n.Attachments = FilesManagementService.GetAttachments(cp.Attachments);
                    list.Add(n);
                }
            }
            else
            {
                var newses = Context.DiplomProjectNewses.OrderByDescending(x => x.EditDate);
                foreach (DiplomProjectNews cp in newses)
                {
                    NewsData n = new NewsData();
                    n.Id = cp.Id;
                    n.Title = cp.Title;
                    n.Body = cp.Body;
                    n.LecturerId = cp.LecturerId;
                    n.DateCreate = cp.EditDate.ToShortDateString();
                    n.Disabled = cp.Disabled;
                    n.PathFile = cp.Attachments;
                    n.Attachments = FilesManagementService.GetAttachments(cp.Attachments);
                    list.Add(n);
                }
            }
           
            return list;
        }

        private string GetGuidFileName()
        {
            return string.Format("P{0}", Guid.NewGuid().ToString("N").ToUpper());
        }

        public DiplomProjectNews SaveNews(DiplomProjectNews news, IList<Attachment> attachments, Int32 userId)
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

            var cpEditNews = Context.DiplomProjectNewses.FirstOrDefault(x => x.Id == news.Id && x.LecturerId == news.LecturerId);
            if (cpEditNews != null)
            {
                CourseProjectNews cpnews = new CourseProjectNews();
                cpEditNews.Body = news.Body;
                cpEditNews.Disabled = news.Disabled;
                cpEditNews.EditDate = news.EditDate;
                cpEditNews.LecturerId = news.LecturerId;
                cpEditNews.Attachments = news.Attachments;
                cpEditNews.Title = news.Title;
            }
            else {
                Context.DiplomProjectNewses.Add(news);
            }
            Context.SaveChanges();

            return news;
        }

        public void DeleteNews(DiplomProjectNews news)
        {
            var cpnews = Context.DiplomProjectNewses.Single(x => x.Id == news.Id && x.LecturerId == news.LecturerId);
            Context.DiplomProjectNewses.Remove(cpnews);
            Context.SaveChanges();
        }

        public void DisableNews(int subjectId, bool disable)
        {
            var models = Context.DiplomProjectNewses.Where(e => e.LecturerId == subjectId);

            foreach (var cpNews in models)
            {
                cpNews.Disabled = disable;
            }
            Context.SaveChanges();
        }

        public DiplomProjectNews GetNews(int id)
        {
            return Context.DiplomProjectNewses.Single(x => x.Id == id);
        }


        private readonly LazyDependency<IDpContext> context = new LazyDependency<IDpContext>();

        private IDpContext Context
        {
            get { return context.Value; }
        }

        private readonly LazyDependency<IFilesManagementService> _filesManagementService =
        new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get { return _filesManagementService.Value; }
        }
    }
}
