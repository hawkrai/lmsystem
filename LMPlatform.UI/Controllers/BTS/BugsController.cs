using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMPlatform.UI.Controllers.BTS
{
    public class BugsController : Controller
    {
        //
        // GET: /BTS/Bugs/

        public ActionResult Index()
        {
            return View("~/Views/BTS/Bugs/Index.cshtml");
        }
    }
}
