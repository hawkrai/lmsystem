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
            var projects = ProjectManagementService.GetUserProjects(WebSecurity.CurrentUserId, pageSize, pageNumber, sortingPropertyName, desc, searchString).Select(e => new ProjectsViewData(e)).ToList();
            int totalCount = ProjectManagementService.GetUserProjectsCount(WebSecurity.CurrentUserId, searchString);
            return new ProjectsResult
            {
                Projects = projects,
                TotalCount = totalCount
            };
        }
    }
}
