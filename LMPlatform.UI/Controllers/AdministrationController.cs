using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Application.Core.UI.Controllers;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.UI.ViewModels.AccountViewModels;
using LMPlatform.UI.ViewModels.AdministrationViewModels;
using Mvc.JQuery.Datatables;

namespace LMPlatform.UI.Controllers
{
  [Authorize(Roles = "admin")]
  public class AdministrationController : BasicController
  {
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Students(int? id)
    {
      if (id.HasValue)
      {
        var student = StudentManagementService.GetStudent(id.Value);

        if (student != null)
        {
          var model = new ModifyStudentViewModel()
            {
              Group = student.Group.Id.ToString(CultureInfo.InvariantCulture),
              Name = student.FirstName,
              Surname = student.LastName,
              Patronymic = student.MiddleName,
              Email = student.Email,
              UserName = student.User.UserName,
            };

          return View("EditStudent", model);
        }
      }

      return View();
    }

    [HttpPost]
    public ActionResult Students(ModifyStudentViewModel model, int id)
    {
      if (ModelState.IsValid)
      {
        try
        {
          model.ModifyStudent(id);
          ViewBag.ResultSuccess = true;
        }
        catch
        {
          ModelState.AddModelError(string.Empty, string.Empty);
        }
      }

      return View("EditStudent", model);
    }

    public ActionResult EditStudent(int id)
    {
      var student = StudentManagementService.GetStudent(id);

      if (student != null)
      {
        var model = new ModifyStudentViewModel
          {
          Group = student.Group.Id.ToString(CultureInfo.InvariantCulture),
          Name = student.FirstName,
          Surname = student.LastName,
          Patronymic = student.MiddleName,
          Email = student.Email,
          UserName = student.User.UserName,
        };

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
          model.ModifyStudent(id);
          ViewBag.ResultSuccess = true;
        }
        catch
        {
          ModelState.AddModelError(string.Empty, string.Empty);
        }
      }

      return null;
    }

    public ActionResult Professors()
    {
      return View();
    }

    public ActionResult AddProfessor()
    {
      var model = new RegisterViewModel();
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
        var model = new ModifyLecturerViewModel
        {
          Name = lecturer.FirstName,
          Surname = lecturer.LastName,
          Patronymic = lecturer.MiddleName,
          UserName = lecturer.User.UserName,
        };

        return PartialView("_EditProfessorView", model);
      }

      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult EditProfessor(ModifyLecturerViewModel model, int id)
    {
      if (ModelState.IsValid)
      {
        try
        {
          model.ModifyLecturer(id);
          ViewBag.ResultSuccess = true;
        }
        catch
        {
          ModelState.AddModelError(string.Empty, string.Empty);
        }
      }

      return null;
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

    public DataTablesResult<StudentViewModel> GetCollectionStudents(DataTablesParam dataTableParam)
    {
      dataTableParam.sSearch = string.Empty;
      ViewBag.EditActionLink = "/Administration/EditStudent";
      var students = StudentManagementService.GetStudents()
        .Select(s => StudentViewModel.FromStudent(s, PartialViewToString("_EditGlyphLinks", s.Id)))
        .AsQueryable();
      return DataTablesResult.Create(students, dataTableParam);
    }

    public DataTablesResult<LecturerViewModel> GetCollectionLecturers(DataTablesParam dataTableParam)
    {
      dataTableParam.sSearch = string.Empty;
      ViewBag.EditActionLink = "/Administration/EditProfessor";
      var lecturers = LecturerManagementService.GetLecturers()
        .Select(l => LecturerViewModel.FormLecturers(l, PartialViewToString("_EditGlyphLinks", l.Id)))
        .AsQueryable();
      return DataTablesResult.Create(lecturers, dataTableParam);
    }

    public DataTablesResult<GroupViewModel> GetCollectionGroups(DataTablesParam dataTableParam)
    {
      dataTableParam.sSearch = string.Empty;
      var groups = GroupManagementService.GetGroups()
        .Select(GroupViewModel.FormGroup)
        .AsQueryable();
      return DataTablesResult.Create(groups, dataTableParam);
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
