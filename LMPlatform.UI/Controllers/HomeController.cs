using System.Web.Mvc;
using Application.Core.UI.Controllers;

namespace LMPlatform.UI.Controllers
{
    public class HomeController : BasicController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Administration");
            }

            return RedirectToAction("Index", "Lms");
        }
    }
}
