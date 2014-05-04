using System.Web.Mvc;

namespace LMPlatform.UI.Controllers
{
    public class DpController : Controller
    {
        // GET: /Dp/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Projects()
        {
            return PartialView();
        }

        public ActionResult Project()
        {
            return PartialView();
        }
    }
}
