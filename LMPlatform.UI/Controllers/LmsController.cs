using System.Web.Mvc;
using Application.Core.UI.Controllers;
using LMPlatform.UI.ViewModels.LmsViewModels;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    using LMPlatform.UI.ViewModels.AdministrationViewModels;

    [Authorize(Roles = "student, lector")]
    public class LmsController : BasicController
    {
        public ActionResult Index()
        {
            if (User.IsInRole("lector") || User.IsInRole("student"))
            {
                var model = new LmsViewModel(WebSecurity.CurrentUserId, User.IsInRole("lector"));
                model.UserActivity = new UserActivityViewModel();
                return View(model);    
            }

            return RedirectToAction("Login", "Account");
        }
    }
}
