using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.BugManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.StudentManagement;
using LMPlatform.Data.Repositories;
using LMPlatform.UI.ViewModels.BTSViewModels;
using Mvc.JQuery.Datatables;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "student, lector")]
    public class BTSController : BasicController
    {
        private static int _currentProjectId;
        
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

        public ActionResult AddProject()
        {
            var projectViewModel = new AddOrEditProjectViewModel(0);
            return PartialView("_AddOrEditProjectForm", projectViewModel);
        }

        [HttpGet]
        public ActionResult AssignUserOnProject()
        {
            var assingUserModel = new AssignUserViewModel(_currentProjectId);
            return PartialView("_AssignUserOnProjectForm", assingUserModel);
        }

        [HttpPost]
        public ActionResult AssignUserOnProject(AssignUserViewModel projectUser)
        {
            if (ModelState.IsValid)
            {
                projectUser.SaveAssignment(_currentProjectId);
            }

            return RedirectToAction("ProjectManagement", new { id = _currentProjectId });
        }

        public ActionResult EditProject(int id)
        {
            var projectViewModel = new AddOrEditProjectViewModel(id);
            return PartialView("_AddOrEditProjectForm", projectViewModel);
        }

        public ActionResult DeleteProject(int id)
        {
            ProjectManagementService.DeleteProject(id);
            return null;
        }

        [HttpPost]
        public ActionResult SaveProject(AddOrEditProjectViewModel model)
        {
            model.Save(WebSecurity.CurrentUserId); 
            return null;
        }

        [HttpGet]
        public ActionResult ProjectManagement(int id)
        {
            var model = new ProjectsViewModel(id);
            _currentProjectId = id;
            return View(model);
        }

        [HttpPost]
        public ActionResult ProjectManagement(string comment)
        {
            var model = new ProjectsViewModel(_currentProjectId);
            model.SaveComment(comment);

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

        public ActionResult AddBug()
        {
            var addBugViewModel = new AddOrEditBugViewModel(0);
            return PartialView("_AddOrEditBugForm", addBugViewModel);
        }

        public ActionResult EditBug(int id)
        {
            var bugViewModel = new AddOrEditBugViewModel(id);
            return PartialView("_AddOrEditBugForm", bugViewModel);
        }

        public ActionResult DeleteBug(int id)
        {
            BugManagementService.DeleteBug(id);
            return null;
        }

        [HttpPost]
        public ActionResult SaveBug(AddOrEditBugViewModel model)
        {
            model.Save(WebSecurity.CurrentUserId);
            return null;
        }

        public ActionResult DeleteProjectUser(int id)
        {
            ProjectManagementService.DeleteProjectUser(id);
            return RedirectToAction("ProjectManagement", "BTS", new RouteValueDictionary { { "id", _currentProjectId } });
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

        [HttpGet]
        public ActionResult BugDetails(int id)
        {
            var model = new BugsViewModel(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult ProjectParticipation()
        {
            var groups = new LmPlatformRepositoriesContainer().GroupsRepository.GetAll();
            var model = new ProjectParticipationViewModel();
            if (!groups.Any())
            {
                model = new ProjectParticipationViewModel(groups.First().Name);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult ProjectParticipation(string groupId)
        {
            var model = new ProjectParticipationViewModel(groupId);
            return View(model);
        }

        [HttpPost]
        public DataTablesResult<ProjectListViewModel> GetProjects(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            var projects = ProjectManagementService.GetProjects(pageInfo: dataTableParam.ToPageInfo(), searchString: searchString);

            return DataTableExtensions.GetResults(projects.Items.Select(model => ProjectListViewModel.FromProject(model, PartialViewToString("_ProjectsGridActions", ProjectListViewModel.FromProject(model)))).Where(e => e.IsAssigned), dataTableParam, projects.TotalCount);
        }

        [HttpPost]
        public DataTablesResult<BugListViewModel> GetAllBugs(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            var bugs = BugManagementService.GetAllBugs(pageInfo: dataTableParam.ToPageInfo(), searchString: searchString);

            return DataTableExtensions.GetResults(bugs.Items.Select(model => BugListViewModel.FromBug(model, PartialViewToString("_BugsGridActions", BugListViewModel.FromBug(model)))), dataTableParam, bugs.TotalCount);
        }

        [HttpPost]
        public DataTablesResult<ProjectUserListViewModel> GetProjectUsers(DataTablesParam dataTablesParam)
        {
            var searchString = dataTablesParam.GetSearchString();
            var projectUsers = ProjectManagementService.GetProjectUsers(pageInfo: dataTablesParam.ToPageInfo(),
                searchString: searchString);
            var projectId = int.Parse(Request.QueryString["projectId"]);

            return DataTableExtensions.GetResults(projectUsers.Items.Select(model => ProjectUserListViewModel.FromProjectUser(model, PartialViewToString("_ProjectUsersGridActions", ProjectUserListViewModel.FromProjectUser(model)))).Where(e => e.ProjectId == projectId && e.UserName != ProjectUserListViewModel.GetProjectCreatorName(projectId)), dataTablesParam, projectUsers.TotalCount);
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
