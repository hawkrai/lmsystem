using System.Web.Mvc;
using Application.Core.UI.Controllers;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "student, lector")]
    public class KnowledgeTestingController : BasicController
    {
        [Authorize, HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize, HttpGet]
        public ActionResult Tests()
        {
            return View();
        }

        [Authorize, HttpGet]
        public ActionResult TestResults()
        {
            return View();
        }
    }
}
