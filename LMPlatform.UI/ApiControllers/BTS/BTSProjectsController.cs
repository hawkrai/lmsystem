using System.Web.Http;
using Application.Core;
using Application.Infrastructure.ProjectManagement;
using System.Web.Mvc;
using Application.Core.Data;
using System.Linq;
using LMPlatform.UI.ViewModels.BTSViewModels;

namespace LMPlatform.UI.ApiControllers.BTS
{
    public class BTSProjectsController : ApiController
    {
        private readonly LazyDependency<IProjectManagementService> projectManagementService = new LazyDependency<IProjectManagementService>();

        public IProjectManagementService ProjectManagementService
        {
            get
            {
                return projectManagementService.Value;
            }
        }

        [System.Web.Http.HttpGet]
        public JsonResult Index(int? pageSize = null, int pageNumber = 1)
        {
            var pageInfo = new PageInfo();
            if (pageSize.HasValue)
            {
                pageInfo.PageSize = pageSize.Value;
            }
            pageInfo.PageNumber = pageNumber;
            var projects = ProjectManagementService.GetProjects(pageInfo: pageInfo).Items.Select(e => new ProjectListViewModel(e)).ToList();
            return new JsonResult()
            {
                Data = new
                {
                    Data = projects
                }
            };
        }
    }
}