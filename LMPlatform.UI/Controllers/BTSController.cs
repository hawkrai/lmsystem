using System.Web.Mvc;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "student, lector")]
    public class BTSController : Controller
    {
        public ActionResult Projects()
        {
            return View();
        }

        public ActionResult ProjectManagement()
        {
            return View();
        }

        public ActionResult ErrorManagement()
        {
            return View();
        }
    }
}
