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

        private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get
            {
                return filesManagementService.Value;
            }
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult IndexV2()
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

        [HttpPost]
        public ActionResult Index(ProjectListViewModel model)
        {
            return View();
        }

        public ActionResult AssignStudentOnProject()
        {
            var projectUserViewModel = new AssignUserViewModel(0, _currentProjectId);
            return PartialView("_AssignStudentOnProjectForm", projectUserViewModel);
        }

        public ActionResult AssignLecturerOnProject()
        {
            var projectUserViewModel = new AssignUserViewModel(0, _currentProjectId);
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

        [HttpDelete]
        public JsonResult DeleteProject(int id)
        {
            ProjectManagementService.DeleteProject(id);
            return Json(id);
        }

        public ActionResult ClearProject()
        {
            ProjectManagementService.ClearProject(_currentProjectId);
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

        //public ActionResult DeleteBug(int id)
        //{
        //    BugManagementService.DeleteBug(id);
        //    return null;
        //}
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

            if (User.IsInRole("lector"))
            {
                return DataTableExtensions.GetResults(projects.Items.Select(model => FromProject(model, PartialViewToString("_ProjectsGridActions", FromProject(model)))).Where(e => e.IsAssigned), dataTableParam, projects.TotalCount);   
            }

            return DataTableExtensions.GetResults(projects.Items.Select(FromProject).Where(e => e.IsAssigned), dataTableParam, projects.TotalCount);
        }

        public ProjectUserListViewModel FromProjectUser(ProjectUser projectUser, string htmlLinks)
        {
            var model = FromProjectUser(projectUser);
            model.Action = new HtmlString(htmlLinks);

            return model;
        }

        public ProjectUserListViewModel FromProjectUser(ProjectUser projectUser)
        {
            var context = new ProjectManagementService();
            
            return new ProjectUserListViewModel
            {
                Id = projectUser.Id,
                UserName = context.GetCreatorName(projectUser.User.Id),
                RoleName = GetRoleName(projectUser.ProjectRoleId),
                ProjectId = projectUser.ProjectId
            };
        }

        public static string GetProjectCreatorName(int projectId)
        {
            var context = new LmPlatformModelsContext();
            var _context = new ProjectManagementService(); 
            var project = context.Projects.Find(projectId);
            var creator = context.Users.Find(project.CreatorId);
            return _context.GetCreatorName(creator.Id);
        }

        public static string GetRoleName(int id)
        {
            var context = new LmPlatformModelsContext();
            var role = context.ProjectRoles.Find(id);
            return role.Name;
        }

        public ProjectListViewModel FromProject(Project project, string htmlLinks)
        {
            var model = FromProject(project);
            model.Action = new HtmlString(htmlLinks);

            return model;
        }

        public ProjectListViewModel FromProject(Project project)
        {
            var context = new LmPlatformModelsContext();
            var isAssigned = false;
            foreach (var user in context.ProjectUsers)
            {
                if (user.ProjectId == project.Id && user.UserId == WebSecurity.CurrentUserId)
                {
                    isAssigned = true;
                }
            }

            var _context = new ProjectManagementService();
            var creatorId = project.Creator.Id;

            return new ProjectListViewModel
            {
                Id = project.Id,
                Title =
                    string.Format("<a href=\"{0}\">{1}</a>", Url.Action("ProjectManagement", "BTS", new { id = project.Id }), project.Title),
                CreatorName = _context.GetCreatorName(creatorId),
                CreationDate = project.DateOfChange.ToShortDateString(),
                UserQuentity = _context.GetProjectUsers(project.Id).Count,
                IsAssigned = isAssigned
            };
        }

        [HttpPost]
        public DataTablesResult<BugListViewModel> GetAllBugs(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            var bugs = BugManagementService.GetAllBugs(pageInfo: dataTableParam.ToPageInfo(), searchString: searchString);

            if (User.IsInRole("lector"))
            {
                if (_currentProjectId != 0)
                {
                    return DataTableExtensions.GetResults(bugs.Items.Select(model => FromBug(model, PartialViewToString("_BugsGridActions", FromBug(model)))).Where(e => e.ProjectId == _currentProjectId), dataTableParam, bugs.TotalCount);
                }

                return DataTableExtensions.GetResults(bugs.Items.Select(model => FromBug(model, PartialViewToString("_BugsGridActions", FromBug(model)))).Where(e => e.IsAssigned), dataTableParam, bugs.TotalCount);
            }

            if (_currentProjectId != 0)
            {
                return DataTableExtensions.GetResults(bugs.Items.Select(FromBug).Where(e => e.ProjectId == _currentProjectId), dataTableParam, bugs.TotalCount);
            }

            return DataTableExtensions.GetResults(bugs.Items.Select(FromBug).Where(e => e.IsAssigned), dataTableParam, bugs.TotalCount);
        }

        public BugListViewModel FromBug(Bug bug, string htmlLinks)
        {
            var model = FromBug(bug);
            model.Action = new HtmlString(htmlLinks);    

            return model;
        }

        public BugListViewModel FromBug(Bug bug)
        {
            var context = new ProjectManagementService();

            var isAssigned = false;
            var user = context.GetProjectsOfUser(WebSecurity.CurrentUserId).Count(e => e.ProjectId == bug.ProjectId && e.UserId == WebSecurity.CurrentUserId);
            if (user != 0)
            {
                isAssigned = true;
            }

            return new BugListViewModel
            {
                Id = bug.Id,
                Steps = bug.Steps,
                Symptom = GetSymptomName(bug.SymptomId),
                ProjectId = bug.ProjectId,
                ReporterName = context.GetCreatorName(bug.ReporterId),
                ReportingDate = bug.ReportingDate.ToShortDateString(),
                Summary = string.Format("<a href=\"{0}\">{1}</a>", Url.Action("BugDetails", "BTS", new { id = bug.Id }), bug.Summary),
                Severity = GetSeverityName(bug.SeverityId),
                Status = GetStatusName(bug.StatusId),
                StatusId = bug.StatusId,
                Project = GetProjectTitle(bug.ProjectId),
                ModifyingDate = bug.ModifyingDate.ToShortDateString(),
                AssignedDeveloperName = (bug.AssignedDeveloperId == 0) ? "отсутствует" : context.GetCreatorName(bug.AssignedDeveloperId),
                IsAssigned = isAssigned
            };
        }

        public string GetProjectTitle(int id)
        {
            var projectManagementService = new ProjectManagementService();
            var project = projectManagementService.GetProject(id);
            return project.Title;
        }

        public string GetReporterName(int id)
        {
            var context = new LmPlatformRepositoriesContainer();
            var user = context.UsersRepository.GetBy(new Query<User>(e => e.Id == id));
            return user.FullName;
        }

        public string GetStatusName(int id)
        {
            var status = new LmPlatformModelsContext().BugStatuses.Find(id);
            return status.Name;
        }

        public string GetSeverityName(int id)
        {
            var severity = new LmPlatformModelsContext().BugSeverities.Find(id);
            return severity.Name;
        }

        public string GetSymptomName(int id)
        {
            var symptom = new LmPlatformModelsContext().BugSymptoms.Find(id);
            return symptom.Name;
        }

        [HttpPost]
        public DataTablesResult<ProjectUserListViewModel> GetProjectUsers(DataTablesParam dataTablesParam)
        {
            var searchString = dataTablesParam.GetSearchString();
            var projectUsers = ProjectManagementService.GetProjectUsers(pageInfo: dataTablesParam.ToPageInfo(),
                searchString: searchString);
            var projectId = int.Parse(Request.QueryString["projectId"]);
            if (User.IsInRole("lector") && ProjectManagementService.GetProject(projectId).CreatorId == WebSecurity.CurrentUserId)
            {
                return DataTableExtensions.GetResults(projectUsers.Items.Select(model => FromProjectUser(model, PartialViewToString("_ProjectUsersGridActions", FromProjectUser(model)))).Where(e => e.ProjectId == projectId && e.UserName != GetProjectCreatorName(projectId)), dataTablesParam, projectUsers.TotalCount);    
            }

            return DataTableExtensions.GetResults(projectUsers.Items.Select(FromProjectUser).Where(e => e.ProjectId == projectId && e.UserName != GetProjectCreatorName(projectId)), dataTablesParam, projectUsers.TotalCount);
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
