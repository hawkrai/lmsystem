using System;
using System.Linq;
using Application.Core;
using Application.Infrastructure.ProjectManagement;
using LMPlatform.UI.Services.Modules.BTS;
using WebMatrix.WebData;
using System.Web.Http;

namespace LMPlatform.UI.Services.BTS
{
    [Authorize(Roles = "student, lector")]
    public class ProjectsService : IProjectsService
    {
        private readonly LazyDependency<IProjectManagementService> projectManagementService = new LazyDependency<IProjectManagementService>();

        public IProjectManagementService ProjectManagementService
        {
            get
            {
                return projectManagementService.Value;
            }
        }

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
    }
}
