using System;
using System.Linq;
using Application.Core;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.BTS;
using LMPlatform.UI.Services.Modules.BTS;
using WebMatrix.WebData;
using System.Web.Http;
using System.IO;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.BTS
{
    [Authorize(Roles = "student, lector")]
    public class ProjectsService : IProjectsService
    {
        public ProjectsResult Index(int pageSize, int pageNumber, string sortingPropertyName, bool desc = false, string searchString = null)
        {
            var projects = ProjectManagementService.GetUserProjects(WebSecurity.CurrentUserId, pageSize, pageNumber, sortingPropertyName, desc, searchString)
                .Select(e => new ProjectViewData(e)).ToList();
            int totalCount = ProjectManagementService.GetUserProjectsCount(WebSecurity.CurrentUserId, searchString);
            return new ProjectsResult
            {
                Projects = projects,
                TotalCount = totalCount
            };
        }

        public ProjectResult Show(string id, bool withDetails)
        {
            int intId = Convert.ToInt32(id);
            ProjectViewData project;
            if (withDetails)
            {
                project = new ProjectViewData(ProjectManagementService
                    .GetProjectWithData(intId, withBugsAndMembers: true), withBugs: true, withMembers: true);
            } else
            {
                project = new ProjectViewData(ProjectManagementService.GetProjectWithData(intId));
            }
            return new ProjectResult
            {
                Project = project
            };
        }

        public UserProjectParticipationsResult ProjectParticipationsByUser(string id, int pageSize, int pageNumber, string sortingPropertyName, bool desc = false, string searchString = null)
        {
            int convertedUserId = Convert.ToInt32(id);
            var projects = ProjectManagementService.GetUserProjectParticipations(convertedUserId, pageSize, pageNumber, sortingPropertyName, desc, searchString)
                .Select(e => new UserProjectParticipationViewData(e, convertedUserId)).ToList();
            int totalCount = ProjectManagementService.GetUserProjectParticipationsCount(convertedUserId, searchString);
            return new UserProjectParticipationsResult
            {
                Projects = projects,
                TotalCount = totalCount
            };
        }

        public StudentsParticipationsResult StudentsParticipationsByGroup(string id, int pageSize, int pageNumber)
        {
            var students = ProjectManagementService.GetStudentsGroupParticipations(int.Parse(id), pageSize, pageNumber)
                .Select(e => new StudentParticipationViewData(e)).ToList();
            int totalCount = ProjectManagementService.GetStudentsGroupParticipationsCount(int.Parse(id));
            return new StudentsParticipationsResult
            {
                ProjectsStudents = students,
                TotalCount = totalCount
            };
        }

        public ProjectCommentsResult GetProjectComments(string id)
        {
            var comments = ProjectManagementService.GetProjectComments(int.Parse(id))
                .Select(e => new ProjectCommentViewData(e)).ToList();
            return new ProjectCommentsResult
            {
                Comments = comments
            };
        }

        public ProjectFileResult SaveFile(string id, ProjectFileViewData projectFile)
        {
            var attachment = new Attachment
            {
                Name = projectFile.Name,
                FileName = projectFile.FileName,
                AttachmentType = (AttachmentType)Enum.Parse(typeof(AttachmentType), projectFile.AttachmentType)
            };
            attachment = ProjectManagementService.SaveAttachment(int.Parse(id), attachment);
            return new ProjectFileResult()
            {
                ProjectFile = new ProjectFileViewData(attachment)
            };
        }

        public ProjectFilesResult GetAttachments(string id)
        {
            var attachments = ProjectManagementService.GetAttachments(int.Parse(id));
            return new ProjectFilesResult()
            {
                ProjectFiles = attachments.Select(e => new ProjectFileViewData(e)).ToList()
            };
        }

        public void DeleteAttachment(string id, string fileName)
        {
            ProjectManagementService.DeleteAttachment(int.Parse(id), fileName);
        }

        public void GenerateMatrix(string id, ProjectMatrixViewData matrix)
        {
            MatrixManagmentService.Fillrequirements(int.Parse(id), matrix.RequirementsFileName);
        }

        private readonly LazyDependency<IProjectManagementService> projectManagementService = new LazyDependency<IProjectManagementService>();
        private readonly LazyDependency<IMatrixManagmentService> matrixManagementService = new LazyDependency<IMatrixManagmentService>();

        private IProjectManagementService ProjectManagementService
        {
            get
            {
                return projectManagementService.Value;
            }
        }

        private IMatrixManagmentService MatrixManagmentService
        {
            get
            {
                return matrixManagementService.Value;
            }
        }
    }
}
