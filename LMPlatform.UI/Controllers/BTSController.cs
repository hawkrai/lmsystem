using System.Linq;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using Application.Infrastructure.ProjectManagement;
using LMPlatform.UI.ViewModels.BTSViewModels;
using Mvc.JQuery.Datatables;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "student, lector")]
    public class BTSController : BasicController
    {
        [HttpGet]
        public ActionResult Projects()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Projects(ProjectsViewModel model)
        {
            model.SaveProject();
            return View();
        }

        public ActionResult ProjectManagement()
        {
            return View();
        }

        public ActionResult ErrorManagement()
        {
            return View();
        }

        public DataTablesResult<ProjectListViewModel> GetCollectionProjects(DataTablesParam dataTableParam)
        {
            dataTableParam.sSearch = string.Empty;
            var projects = ProjectManagementService.GetProjects()
              .Select(ProjectListViewModel.FromProject)
              .AsQueryable();
            return DataTablesResult.Create(projects, dataTableParam);
        }

        public IProjectManagementService ProjectManagementService
        {
            get
            {
                return ApplicationService<IProjectManagementService>();
            }
        }
    }
}
