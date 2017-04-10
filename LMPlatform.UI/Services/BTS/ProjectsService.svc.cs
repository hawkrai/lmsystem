using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
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

        public ProjectResult Show(string id)
        {
            var project = new ProjectViewData(ProjectManagementService.GetProjectWithData(Convert.ToInt32(id)));
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
    }
}
