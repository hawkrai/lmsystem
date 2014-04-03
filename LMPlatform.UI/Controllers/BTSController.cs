using System;
using System.Linq;
using System.Web.Mvc;
using Application.Core.Data;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.BugManagement;
using Application.Infrastructure.ProjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.ViewModels.BTSViewModels;
using Mvc.JQuery.Datatables;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "student, lector")]
    public class BTSController : BasicController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ProjectListViewModel model)
        {
            //model.SaveProject();
            return View();
        }

        [HttpGet]
        public ActionResult AddProject()
        {
            var projectViewModel = new AddProjectViewModel
            {
                CreatorId = WebSecurity.CurrentUserId
            };

            return PartialView("_AddProjectForm", projectViewModel);
        }

        [HttpPost]
        public ActionResult AddProject(AddProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                project.SaveProject();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ProjectManagement()
        {
            var model = new ProjectsViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProjectManagement(int projectId)
        {
            var model = new ProjectsViewModel(projectId);
            return View(model);
        }

        [HttpGet]
        public ActionResult BugManagement()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BugManagement(BugListViewModel model)
        {
            return View();
        }

        [HttpGet]
        public ActionResult DocumentBug()
        {
            var addBugViewModel = new AddBugViewModel();

            return PartialView("_AddBugForm", addBugViewModel);
        }

        [HttpPost]
        public ActionResult DocumentBug(AddBugViewModel bug)
        {
            if (ModelState.IsValid)
            {
                bug.SaveBug();
            }

            return RedirectToAction("BugManagement");
        }

        [HttpPost]
        public DataTablesResult<ProjectListViewModel> GetProjects(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            var projects = ProjectManagementService.GetProjects(pageInfo: dataTableParam.ToPageInfo(),
                searchString: searchString);

            return DataTableExtensions.GetResults(projects.Items.Select(ProjectListViewModel.FromProject), dataTableParam, projects.TotalCount);
        }

        [HttpPost]
        public DataTablesResult<ProjectUserListViewModel> GetProjectUsers(DataTablesParam dataTablesParam)
        {
            var searchString = dataTablesParam.GetSearchString();
            var projectUsers = ProjectManagementService.GetProjectUsers(pageInfo: dataTablesParam.ToPageInfo(),
                searchString: searchString);

            return DataTableExtensions.GetResults(projectUsers.Items.Select(ProjectUserListViewModel.FromProjectUser), dataTablesParam, projectUsers.TotalCount);
        }

        [HttpPost]
        public DataTablesResult<BugListViewModel> GetAllBugs(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            var bugs = BugManagementService.GetAllBugs(pageInfo: dataTableParam.ToPageInfo(),
                searchString: searchString);

            return DataTableExtensions.GetResults(bugs.Items.Select(BugListViewModel.FromBug), dataTableParam, bugs.TotalCount);
        }

        public IProjectManagementService ProjectManagementService
        {
            get
            {
                return ApplicationService<IProjectManagementService>();
            }
        }

        public IBugManagementService BugManagementService
        {
            get
            {
                return ApplicationService<IBugManagementService>();
            }
        }
    }
}
