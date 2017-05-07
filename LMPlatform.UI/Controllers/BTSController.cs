using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core;
using Application.Core.Data;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.BugManagement;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.Data.Infrastructure;
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
        private static int _currentBugId;
        private static int _prevBugStatus;
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Bugs()
        {
            return PartialView("_Bugs");
        }

        [HttpGet]
        public ActionResult Projects()
        {
            return PartialView("_Projects");
        }

        [HttpGet]
        public ActionResult ProjectParticipation()
        {
            return PartialView("_ProjectParticipation");
        }

        [HttpGet]
        public ActionResult Project()
        {
            return PartialView("_Project");
        }

        public ActionResult AssignStudentOnProject(int id)
        {
            var projectUserViewModel = new AssignUserViewModel(0, id);
            return PartialView("_AssignStudentOnProjectForm", projectUserViewModel);
        }

        public ActionResult AssignLecturerOnProject(int id)
        {
            var projectUserViewModel = new AssignUserViewModel(0, id);
            return PartialView("_AssignLecturerOnProjectForm", projectUserViewModel);
        }

        [HttpDelete]
        public JsonResult DeleteProjectUser(int id)
        {
            ProjectManagementService.DeleteProjectUser(id);
            return Json(id);
        }

        [HttpPost]
        public ActionResult SaveProjectUser(AssignUserViewModel model)
        {
            model.SaveAssignment();

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

        [HttpDelete]
        public JsonResult DeleteProject(int id)
        {
            ProjectManagementService.DeleteProject(id);
            return Json(id);
        }

        public ActionResult ClearProject(int id)
        {
            ProjectManagementService.ClearProject(id);
            return null;
        }

        [HttpPost]
        public ActionResult SaveProject(AddOrEditProjectViewModel model)
        {
            model.Save(WebSecurity.CurrentUserId); 
            return null;
        }

        [HttpPost]
        public ActionResult ProjectManagement(int id, string comment)
        {
            var model = new ProjectsViewModel(id);
            model.SaveComment(comment);

            return PartialView("_ChatForm", model);
        }

        [HttpGet]
        public ActionResult BugManagement(int id)
        {
            _currentProjectId = id;
            var model = new BugListViewModel(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult BugManagement(BugListViewModel model)
        {
            return View();
        }

        public ActionResult AddBug()
        {
            var addBugViewModel = new AddOrEditBugViewModel(0);
            if (_currentProjectId == 0)
            {
                return PartialView("_AddBugForm", addBugViewModel);
            }
                
            return PartialView("_AddBugOrCurrentProjectForm", addBugViewModel);
        }

        public ActionResult EditBug(int id)
        {
            var bug = BugManagementService.GetBug(id);
            _currentProjectId = bug.ProjectId;

            var bugViewModel = new AddOrEditBugViewModel(id);
            _prevBugStatus = bugViewModel.StatusId;
            var projectUser =
                new ProjectManagementService().GetProjectUsers(bug.ProjectId).Single(e => e.UserId == WebSecurity.CurrentUserId);
            if ((projectUser.ProjectRoleId == 1 && bug.StatusId == 2) ||
                (projectUser.ProjectRoleId == 3 && bug.StatusId == 1))
            {
                return PartialView("_EditBugFormWithAssignment", bugViewModel);
            }

            return PartialView("_EditBugFormWithAssignment", bugViewModel);
        }

        [HttpDelete]
        public JsonResult DeleteBug(int id)
        {
            BugManagementService.DeleteBug(id);
            return Json(id);
        }

        [HttpPost]
        public ActionResult SaveBug(AddOrEditBugViewModel model)
        {
            model.Save(WebSecurity.CurrentUserId, _currentProjectId);
            var bugLog = new BugLog
            {
                BugId = model.BugId,
                UserId = WebSecurity.CurrentUserId,
                UserName = ProjectManagementService.GetCreatorName(WebSecurity.CurrentUserId),
                PrevStatusId = _prevBugStatus,
                CurrStatusId = model.StatusId,
                LogDate = DateTime.Now
            };
            if (model.BugId != 0)
            {
                model.SaveBugLog(bugLog);
            }

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
        public JsonResult GetDeveloperNames()
        {
            var _context = new UsersManagementService();
            var context = new ProjectManagementService();
            var projectUsers = context.GetProjectUsers(_currentProjectId).ToList().Where(e => e.ProjectRoleId == 1);

            var users = new List<User>();

             var currProjectUser =
                context.GetProjectUsers(_currentProjectId).Single(e => e.UserId == WebSecurity.CurrentUserId);
            if (currProjectUser.ProjectRoleId == 1)
            {
                users.Add(_context.GetUser(currProjectUser.UserId));
            }
            else
            {
                foreach (var user in projectUsers)
                {
                    users.Add(_context.GetUser(user.UserId));
                }
            }

            var userList = users.Select(e => new SelectListItem
            {
                Text = context.GetCreatorName(e.Id),
                Value = e.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            return Json(new SelectList(userList, "Value", "Text"));
        }

        [HttpPost]
        public bool IsUserAnAssignedDeveloper()
        {
            var bug = new BugManagementService().GetBug(_currentBugId);
            var context = new ProjectManagementService();
            var projectRoleId = context.GetProjectUsers(bug.ProjectId).Single(e => e.UserId == WebSecurity.CurrentUserId).ProjectRoleId;
            if (bug.AssignedDeveloperId == 0 && projectRoleId == 1)
            {
                return true;
            }
            else
            {
                if (bug.AssignedDeveloperId != WebSecurity.CurrentUserId && projectRoleId == 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [HttpPost]
        public JsonResult GetLecturers()
        {
            var _lecturers = new LecturerManagementService().GetLecturers();

            var lecturerList = new List<Lecturer>();
            foreach (var lecturer in _lecturers)
            {
                if (ProjectManagementService.IsUserAssignedOnProject(lecturer.Id, _currentProjectId) == false)
                {
                    lecturerList.Add(lecturer);
                }
            }

            var lecturers = lecturerList.Select(v => new SelectListItem
            {
                Text = v.FullName,
                Value = v.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            return Json(new SelectList(lecturers, "Value", "Text"));
        }

        [HttpPost]
        public ActionResult GetUserInformation(int id)
        {
            var model = new UserInfoViewModel(id);
            return PartialView("_UserInfo", model);
        }

        [HttpPost]
        public ActionResult GetBugStatusHelper()
        {
            return PartialView("_BugStatusHelper");
        }

        [HttpGet]
        public ActionResult BugDetails(int id)
        {
            var model = new BugsViewModel(id);
            _currentBugId = id;
            return View(model);
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
