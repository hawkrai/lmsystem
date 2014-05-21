using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.BugManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.StudentManagement;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
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

        public ActionResult AssignUserOnProject()
        {
            var projectUserViewModel = new AssignUserViewModel(0, _currentProjectId);
            return PartialView("_AssignUserOnProjectForm", projectUserViewModel);
        }

        public ActionResult DeleteProjectUser(int id)
        {
            ProjectManagementService.DeleteProjectUser(id);
            return null;
        }

        [HttpPost]
        public ActionResult SaveProjectUser(AssignUserViewModel model)
        {
            model.SaveAssignment(_currentProjectId);
            return null;
        }

        public ActionResult AddProject()
        {
            var projectViewModel = new AddOrEditProjectViewModel(0);
            return PartialView("_AddOrEditProjectForm", projectViewModel);
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

            return PartialView("_ChatForm", model);
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
        
        [HttpPost]
        public JsonResult GetStudents(int groupId)
        {
            var groupOfStudents = new StudentManagementService().GetGroupStudents(groupId);
            var studentList = new List<Student>();
            foreach (var student in groupOfStudents)
            {
                if (ProjectManagementService.IsUserAssignedOnProject(student.Id, _currentProjectId) == false)
                {
                    studentList.Add(student);
                }
            }

            var students = studentList.Select(v => new SelectListItem
            {
                Text = v.FullName,
                Value = v.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            return Json(new SelectList(students, "Value", "Text"));
        }

        [HttpPost]
        public ActionResult GetUserInformation(int id)
        {
            var model = new UserInfoViewModel(id);
            return PartialView("_UserInfo", model);
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
            var model = new ProjectParticipationViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProjectParticipation(int groupId)
        {
            var model = new ProjectParticipationViewModel(groupId);
            return PartialView("_studentProjectsList", model);
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
