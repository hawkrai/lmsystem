using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Helpers;
using System.Web.Mvc;
using Application.Core;
using Application.Core.Data;
using Application.Core.Extensions;
using Application.Core.UI.Controllers;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models;
using LMPlatform.UI.ViewModels.LmsViewModels;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
	using Application.Infrastructure.StudentManagement;
    using LMPlatform.UI.Attributes;
    using ViewModels.AdministrationViewModels;

    [JwtAuth(Roles = "student, lector")]
    public class LmsController : BasicController
    {
		public ActionResult Index(string userLogin = "")
        {
            if (User.IsInRole("lector") || User.IsInRole("student"))
            {
                var model = new LmsViewModel(WebSecurity.CurrentUserId, User.IsInRole("lector"));
                model.UserActivity = new UserActivityViewModel();

                ViewBag.ShowDpSectionForUser = DpManagementService.ShowDpSectionForUser(WebSecurity.CurrentUserId);

				bool isMyProfile = string.IsNullOrEmpty(userLogin) || WebSecurity.CurrentUserName == userLogin;

	            ViewData["isMyProfile"] = isMyProfile;
				ViewData["userLogin"] = string.IsNullOrEmpty(userLogin) || WebSecurity.CurrentUserName == userLogin ? WebSecurity.CurrentUserName : userLogin;

				ViewData["UnconfirmedStudents"] = this.StudentManagementService.CountUnconfirmedStudents(WebSecurity.CurrentUserId) > 0;

                return View(model);    
            }

            return RedirectToAction("Login", "Account");
        }

        private IDpManagementService DpManagementService
        {
            get { return _diplomProjectManagementService.Value; }
        }

        private readonly LazyDependency<IDpManagementService> _diplomProjectManagementService = new LazyDependency<IDpManagementService>();

		private IStudentManagementService StudentManagementService
		{
			get { return _studentManagementService.Value; }
		}

		private readonly LazyDependency<IStudentManagementService> _studentManagementService = new LazyDependency<IStudentManagementService>();
    }
}
