using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core.UI.Controllers;

namespace LMPlatform.UI.Controllers
{
    public class ChatController : BasicController
    {
        //
        // GET: /Chat/
        public ActionResult Index()
        {
            return View("Chat", "Layouts/_MainLayout");
        }

        
    }
}
