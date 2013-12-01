using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Application.Core.UI.Controllers;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.StudentManagement;
using LMPlatform.Models;
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
          if (model.IsPasswordReset)
          {
            model.ResetPassword();
          }

          ViewBag.ResultSuccess = true;
        }
        catch (MembershipCreateUserException e)
        {
          ModelState.AddModelError(string.Empty, e.StatusCode.ToString());
        }
      }

      return Students(id);
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

    public DataTablesResult<StudentViewModel> GetCollectionStudents(DataTablesParam dataTableParam)
    {
      dataTableParam.sSearch = string.Empty;
      var students = StudentManagementService.GetStudents()
        .Select(s => StudentViewModel.FromStudent(s, PartialViewToString("_EditGlyphLinks", s.Id)))
        .AsQueryable();
      return DataTablesResult.Create(students, dataTableParam);
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
    #endregion
  }
}
