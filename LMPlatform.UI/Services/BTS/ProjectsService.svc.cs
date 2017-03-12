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

namespace LMPlatform.UI.Services.BTS
{
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

        public ProjectsResult Index(int pageSize, int pageNumber = 0)
        {
            var projects = ProjectManagementService.GetUserProjects(WebSecurity.CurrentUserId, pageSize, pageNumber).Select(e => new ProjectsViewData(e)).ToList();
            return new ProjectsResult
            {
                Projects = projects
            };
        }
    }
}
