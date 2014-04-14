using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Application.Core.Data;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.BugManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.StudentManagement;
using LMPlatform.Models;
using LMPlatform.UI.ViewModels.BTSViewModels;
using Mvc.JQuery.Datatables;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "student, lector")]
    public class BTSController : BasicController
    {
        private static int currentProjectId = 0;
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ProjectListViewModel model)
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddProject()
        {
            var projectViewModel = new AddOrEditProjectViewModel();
            return PartialView("_AddOrEditProjectForm", projectViewModel);
        }

        [HttpPost]
        public ActionResult AddProject(AddOrEditProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                project.SaveProject();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AssignUserOnProject()
        {
            var assingUserModel = new AssignUserViewModel(currentProjectId);
            return PartialView("_AssignUserOnProjectForm", assingUserModel);
        }

        [HttpPost]
        public ActionResult AssignUserOnProject(AssignUserViewModel projectUser)
        {
            if (ModelState.IsValid)
            {
                projectUser.SaveAssignment();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditProject(int projectId)
        {
            var project = ProjectManagementService.GetProject(projectId);
            var projectViewModel = new AddOrEditProjectViewModel(project);
            return PartialView("_AddOrEditProjectForm", projectViewModel);
        }

        [HttpPost]
        public ActionResult EditProject(AddOrEditProjectViewModel project, int projectId)
        {
            if (ModelState.IsValid)
            {
                project.UpdateProject(projectId);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ProjectManagement(int projectId)
        {
            var model = new ProjectsViewModel(projectId);
            currentProjectId = projectId;
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

        public ActionResult DeleteProject(int projectId)
        {
            ProjectManagementService.DeleteProject(projectId);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteProjectUser(int projectUserId)
        {
            ProjectManagementService.DeleteProjectUser(projectUserId);
            return RedirectToAction("ProjectManagement", "BTS", new RouteValueDictionary { { "projectId", currentProjectId } });
        }

        [HttpPost]
        public JsonResult GetStudents(int groupId)
        {
            var students = new StudentManagementService().GetGroupStudents(groupId).Select(v => new SelectListItem
            {
                Text = v.FullName,
                Value = v.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            return Json(new SelectList(students, "Value", "Text"));
        }

        [HttpPost]
        public DataTablesResult<ProjectListViewModel> GetProjects(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            var projects = ProjectManagementService.GetProjects(pageInfo: dataTableParam.ToPageInfo(),
                searchString: searchString);

            return DataTableExtensions.GetResults(projects.Items.Select(model => ProjectListViewModel.FromProject(model, PartialViewToString("_ProjectsGridActions", ProjectListViewModel.FromProject(model)))), dataTableParam, projects.TotalCount);
        }

        [HttpPost]
        public DataTablesResult<ProjectUserListViewModel> GetProjectUsers(DataTablesParam dataTablesParam)
        {
            var searchString = dataTablesParam.GetSearchString();
            var projectUsers = ProjectManagementService.GetProjectUsers(pageInfo: dataTablesParam.ToPageInfo(),
                searchString: searchString);
            var projectId = int.Parse(Request.QueryString["projectId"]);

            return DataTableExtensions.GetResults(projectUsers.Items.Select(model => ProjectUserListViewModel.FromProjectUser(model, PartialViewToString("_ProjectUsersGridActions", ProjectUserListViewModel.FromProjectUser(model)))).Where(e => e.ProjectId == projectId), dataTablesParam, projectUsers.TotalCount);
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
