using System.Web.Mvc;
using System.Web.Security;
using Application.Core.UI.Controllers;
using Application.Infrastructure.UserManagement;
using LMPlatform.Models;
using LMPlatform.UI.ViewModels.AdministrationViewModels;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    using LMPlatform.UI.ViewModels.AccountViewModels;

    public class AccountController : BasicController
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            ActionResult result;

            if (ModelState.IsValid && model.Login())
            {
                if (User.IsInRole("admin"))
                {
                    result = RedirectToAction("Management");                
                }

                UsersManagementService.UpdateLastLoginDate(model.UserName); 

                result = _RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Имя пользователя или пароль не являются корректными");

                result = View(model);
            }

            return result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var loginViewModel = new LoginViewModel();
            loginViewModel.LogOut();

            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        public ActionResult PersonalData()
        {
            var model = new PersonalDataViewModel();

            return PartialView("_PersonalData", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.RegistrationUser(new[] { "student" });

                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, ErrorCodeToString(e.StatusCode));
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Management()
        {
            ViewBag.ReturnUrl = Url.Action("Management");
			var model = new PersonalDataViewModel();
	        ViewBag.Avatar = model.Avatar;
            return View();
        }

        private ActionResult _RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl)
                ? (ActionResult)Redirect(returnUrl)
                : RedirectToAction("Index", "Home");
        }

        [HttpPost]
		public JsonResult UpdatePerconalData(PersonalDataViewModel model, string avatar)
        {
            if (Roles.IsUserInRole("lector"))
            {
                var modData = new ModifyLecturerViewModel(new Lecturer
                {
                    FirstName = model.Name,
                    LastName = model.Patronymic,
                    MiddleName = model.Surname,
                    User = new User
                    {
                        UserName = model.UserName,
						Avatar = avatar,
                        Id = WebSecurity.CurrentUserId
                    },
                    Id = WebSecurity.CurrentUserId
                });

                modData.ModifyLecturer();
            }
            else
            {
                var modData = new ModifyStudentViewModel(new Student
                {
                    FirstName = model.Name,
                    LastName = model.Patronymic,
                    MiddleName = model.Surname,
                    User = new User
                    {
                        UserName = model.UserName,
						Avatar = avatar,
                        Id = WebSecurity.CurrentUserId
                    },
                    Id = WebSecurity.CurrentUserId
                }); 
   
                modData.ModifyStudent();
            }

            return Json(true);
        }

		public string GetAvatar()
		{
			var model = new PersonalDataViewModel();
			return model.Avatar;
		}

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        public IUsersManagementService UsersManagementService
        {
            get
            {
                return ApplicationService<IUsersManagementService>();
            }
        }
    }
}
