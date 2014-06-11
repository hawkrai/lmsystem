using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.UI.ViewModels;
using LMPlatform.UI.ViewModels.AccountViewModels;
using LMPlatform.UI.ViewModels.AdministrationViewModels;
using Mvc.JQuery.Datatables;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdministrationController : BasicController
    {
        public ActionResult Index()
        {
            var activityModel = new UserActivityViewModel();
            return View(activityModel);
        }

        public ActionResult Students()
        {
            return View();
        }

        public ActionResult EditStudent(int id)
        {
            var student = StudentManagementService.GetStudent(id);

            if (student != null)
            {
                var model = new ModifyStudentViewModel(student);
                return PartialView("_EditStudentView", model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditStudent(ModifyStudentViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.ModifyStudent();
                    ViewBag.ResultSuccess = true;
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, string.Empty);
                }
            }

            return null;
        }

        [HttpPost]
        public ActionResult EditStudentAjax(ModifyStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = UsersManagementService.GetUserByName(model.Name, model.Surname, model.Patronymic);
                    if (user == null || user.Id == model.Id)
                    {
                        model.ModifyStudent();
                        return Json(new { resultMessage = "Студент сохранен" });
                    }

                    ModelState.AddModelError(string.Empty, "Пользователь с таким именем уже существует");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, string.Empty);
                }
            }

            return PartialView("_EditStudentView", model);
        }

        public ActionResult AddProfessor()
        {
            var model = new RegisterViewModel();
            return PartialView("_AddProfessorView", model);
        }

        [HttpPost]
        public ActionResult AddProfessorAjax(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = UsersManagementService.GetUserByName(model.Name, model.Surname, model.Patronymic);
                    if (user == null)
                    {
                        model.RegistrationUser(new[] { "lector" });
                        return Json(new { resultMessage = "Преподаватель сохранен" });
                    }

                    ModelState.AddModelError(string.Empty, "Пользователь с таким именем уже существует");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, e.StatusCode.ToString());
                }
            }

            return PartialView("_AddProfessorView", model);
        }

        [HttpPost]
        public ActionResult AddProfessor(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.RegistrationUser(new[] { "lector" });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, e.StatusCode.ToString());
                    return View(model);
                }
            }

            return null;
        }

        public ActionResult EditProfessor(int id)
        {
            var lecturer = LecturerManagementService.GetLecturer(id);

            if (lecturer != null)
            {
                var model = new ModifyLecturerViewModel(lecturer);
                return PartialView("_EditProfessorView", model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditProfessor(ModifyLecturerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.ModifyLecturer();
                    ViewBag.ResultSuccess = true;
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, string.Empty);
                }
            }

            return null;
        }

        [HttpPost]
        public ActionResult EditProfessorAjax(ModifyLecturerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = UsersManagementService.GetUserByName(model.Name, model.Surname, model.Patronymic);
                    if (user == null || user.Id == model.LecturerId)
                    {
                        model.ModifyLecturer();
                        return this.Json(new { resultMessage = "Преподаватель сохранен" });
                    }

                    ModelState.AddModelError(string.Empty, "Пользователь с таким именем уже существует");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, string.Empty);
                }
            }

            return PartialView("_EditProfessorView", model);
        }

        public ActionResult AddGroup()
        {
            var model = new GroupViewModel();
            return PartialView("_AddGroupView", model);
        }

        [HttpPost]
        public ActionResult AddGroup(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!model.CheckGroupName())
                    {
                        ModelState.AddModelError(string.Empty, "Группа с таким номером уже существует");
                    }
                    else
                    {
                        model.AddGroup();
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, e.StatusCode.ToString());
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddGroupAjax(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!model.CheckGroupName())
                    {
                        ModelState.AddModelError(string.Empty, "Группа с таким номером уже существует");
                    }
                    else
                    {
                        model.AddGroup();
                        return Json(new { resultMessage = "Группа сохранена" });
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, e.StatusCode.ToString());
                }
            }

            return PartialView("_AddGroupView", model);
        }

        public ActionResult EditGroup(int id)
        {
            var group = GroupManagementService.GetGroup(id);

            if (group != null)
            {
                var model = new GroupViewModel(group);
                return PartialView("_EditGroupView", model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditGroup(GroupViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.ModifyGroup();
                    ViewBag.ResultSuccess = true;
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, string.Empty);
                }
            }

            return null;
        }

        [HttpPost]
        public ActionResult EditGroupAjax(GroupViewModel model)
        {
            if (!model.CheckGroupName())
            {
                this.ModelState.AddModelError(string.Empty, "Группа с таким именем уже существует");
            }

            if (ModelState.IsValid && model.CheckGroupName())
            {
                try
                {
                    if (!model.CheckGroupName())
                    {
                        this.ModelState.AddModelError(string.Empty, "Группа с таким именем уже существует");
                    }
                    else
                    {
                        model.ModifyGroup();
                        return Json(new { resultMessage = "Группа сохранена" });
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Произошла ошибка");
                }
            }

            return PartialView("_EditGroupView", model);
        }

        public ActionResult Professors()
        {
            return View();
        }

        public ActionResult Groups()
        {
            return View();
        }

        public ActionResult Files()
        {
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            var login = id;
            try
            {
                var user = UsersManagementService.GetUser(login);

                var resetPassModel = new ResetPasswordViewModel(user);
                if (Request != null && Request.UrlReferrer != null)
                {
                    ViewBag.ReturnUrl = Request.UrlReferrer;
                }
                else
                {
                    ViewBag.ReturnUrl = "/Administration/Index";
                }

                return View(resetPassModel);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resetResult = model.ResetPassword();
                ViewBag.ResultSuccess = resetResult;

                if (!resetResult)
                {
                    ModelState.AddModelError(string.Empty, "Пароль не был сброшен");
                }
            }

            return View(model);
        }

        [HttpPost]
        public bool DeleteUser(int id)
        {
            try
            {
                UsersManagementService.DeleteUser(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public JsonResult DeleteStudent(int id)
        {
            try
            {
                var student = StudentManagementService.GetStudent(id);
                if (student != null)
                {
                    var result = StudentManagementService.DeleteStudent(id);
                    if (result)
                    {
                        return Json(new { resultMessage = string.Format("Студент {0} удален", student.FullName) });
                    }

                    return Json(new { resultMessage = string.Format("Не удалось удалить студента {0}", student.FullName) });
                }

                return Json(new { resultMessage = "Удаление невозможно. Студента не существует" });
            }
            catch
            {
                return Json(new { resultMessage = "Произошла ошибка при удалении" });
            }
        }

        [HttpPost]
        public JsonResult DeleteLecturer(int id)
        {
            try
            {
                var lecturer = LecturerManagementService.GetLecturer(id);

                if (lecturer != null)
                {
                    var result = LecturerManagementService.DeleteLecturer(id);

                    if (result)
                    {
                        return Json(new { resultMessage = string.Format("Преподаватель {0} удален", lecturer.FullName) });
                    }

                    return Json(new { resultMessage = string.Format("Не удалось удалить преподавателя {0}", lecturer.FullName) });
                }

                return Json(new { resultMessage = "Удаление невозможно. Преподавателя не существует" });
            }
            catch
            {
                return Json(new { resultMessage = "Произошла ошибка при удалении" });
            }
        }

        [HttpPost]
        public JsonResult DeleteGroup(int id)
        {
            try
            {
                var group = GroupManagementService.GetGroup(id);
                if (group != null)
                {
                    if (group.Students != null && group.Students.Count > 0)
                    {
                        return Json(new { resultMessage = "Группа содержит студентов и не может быть удалена" });
                    }

                    GroupManagementService.DeleteGroup(id);
                    return Json(new { resultMessage = string.Format("Группа {0} удалена", group.Name) });
                }

                return Json(new { resultMessage = "Группы не существует" });
            }
            catch
            {
                return Json(new { resultMessage = "Произошла ошибка при удалении" });
            }
        }

        public JsonResult Attendance(int id)
        {
            var user = UsersManagementService.GetUser(id);

            if (user != null && user.Attendance != null)
            {
                var data = user.AttendanceList.GroupBy(e => e.Date).Select(d => new { day = d.Key.ToString("d"), count = d.Count() });
                return Json(new { resultMessage = user.FullName, attendance = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { resultMessage = "Нет данных", data = "[]" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public DataTablesResult<StudentViewModel> GetCollectionStudents(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            ViewBag.EditActionLink = "/Administration/EditStudent";
            ViewBag.DeleteActionLink = "/Administration/DeleteStudent";
            ViewBag.StatActionLink = "/Administration/Attendance";
            var students = StudentManagementService.GetStudentsPageable(pageInfo: dataTableParam.ToPageInfo(), searchString: searchString);
            return DataTableExtensions.GetResults(students.Items.Select(s => StudentViewModel.FromStudent(s, PartialViewToString("_EditGlyphLinks", s.Id))), dataTableParam, students.TotalCount);
        }

        [HttpPost]
        public DataTablesResult<LecturerViewModel> GetCollectionLecturers(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            ViewBag.EditActionLink = "/Administration/EditProfessor";
            ViewBag.DeleteActionLink = "/Administration/DeleteLecturer";
            ViewBag.StatActionLink = "/Administration/Attendance";
            var lecturers = LecturerManagementService.GetLecturersPageable(pageInfo: dataTableParam.ToPageInfo(), searchString: searchString);
            return DataTableExtensions.GetResults(lecturers.Items.Select(l => LecturerViewModel.FormLecturers(l, PartialViewToString("_EditGlyphLinks", l.Id))), dataTableParam, lecturers.TotalCount);
        }

        [HttpPost]
        public DataTablesResult<GroupViewModel> GetCollectionGroups(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            ViewBag.EditActionLink = "/Administration/EditGroup";
            ViewBag.DeleteActionLink = "/Administration/DeleteGroup";
            var groups = GroupManagementService.GetGroupsPageable(pageInfo: dataTableParam.ToPageInfo(), searchString: searchString);
            return DataTableExtensions.GetResults(groups.Items.Select(g => GroupViewModel.FormGroup(g, PartialViewToString("_EditGlyphLinks", g.Id))), dataTableParam, groups.TotalCount);
        }

        #region Dependencies

        public IStudentManagementService StudentManagementService
        {
            get
            {
                return ApplicationService<IStudentManagementService>();
            }
        }

        public IGroupManagementService GroupManagementService
        {
            get
            {
                return ApplicationService<IGroupManagementService>();
            }
        }

        public ILecturerManagementService LecturerManagementService
        {
            get
            {
                return ApplicationService<ILecturerManagementService>();
            }
        }

        public IUsersManagementService UsersManagementService
        {
            get
            {
                return ApplicationService<IUsersManagementService>();
            }
        }
        #endregion
    }
}
