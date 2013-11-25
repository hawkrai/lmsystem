using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.StudentManagement;
using LMPlatform.Models;
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

    public ActionResult Students()
    {
      return View();
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
        .Select(StudentViewModel.FromStudent)
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
