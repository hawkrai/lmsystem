using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core.Constants;
using Application.Core.UI.Controllers;

namespace LMPlatform.UI.Controllers
{
    public class ChatController : BasicController
    {
        //
        // GET: /Chat/
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole(Constants.Roles.Admin))
            {
                return View("Chat", "Layouts/_AdministrationLayout");
            }

            return View("Chat", "Layouts/_MainUsingNavLayout");
        }

        
    }
}
