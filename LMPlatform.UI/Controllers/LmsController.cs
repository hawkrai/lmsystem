using System.Web.Mvc;
using Application.Core.UI.Controllers;

namespace LMPlatform.UI.Controllers
{
   public class LmsController : BasicController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
