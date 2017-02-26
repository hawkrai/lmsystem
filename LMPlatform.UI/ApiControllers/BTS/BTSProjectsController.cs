using System.Web.Http;
using Application.Core;
using Application.Infrastructure.ProjectManagement;
using System.Web.Mvc;
using Application.Core.Data;
using System.Linq;
using LMPlatform.UI.ViewModels.BTSViewModels;
using WebMatrix.WebData;

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
        public JsonResult Index(int pageSize = 0, int pageNumber = 1)
        {
            var pageInfo = new PageInfo();
            pageInfo.PageSize = pageSize;
            pageInfo.PageNumber = pageNumber;
            var projects = ProjectManagementService.GetUserProjects(WebSecurity.CurrentUserId, pageInfo: pageInfo).Items.Select(e => new ProjectListViewModel(e)).ToList();
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