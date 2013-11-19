using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core.UI.Controllers;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdministrationController : BasicController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
