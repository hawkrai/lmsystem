using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core.Constants;
using Application.Core.UI.Controllers;
using LMPlatform.Models;
using LMPlatform.UI.Attributes;
//using TwitterBootstrapMVC;

namespace LMPlatform.UI.Controllers
{
    public class ChatController : BasicController
    {
        //
        // GET: /Chat/
        public static bool EnabledChat { get; set; }
      
        [JwtAuth]
        public ActionResult Index()
        {
            if (User.IsInRole(Constants.Roles.Admin))
            {
                return View("Chat", "Layouts/_AdministrationLayout");
            }
            if (EnabledChat)
            {
                return View("Chat", "Layouts/_MainUsingNavLayout");
            }
            else
            {
                return RedirectToAction("Index", "Lms");
            }
        }
        
        [JwtAuth(Roles = "admin")]
        [HttpPost]
        public JsonResult EnablChat(string Switcher)
        {
            EnabledChat = Convert.ToBoolean(Switcher);
            if (!EnabledChat)
            {
                return Json(new {resultMessage = "Чат отключен"});
            }
            else
            {
                return Json(new {resultMessage = "Чат включен"});
            }
        }
      
    }
}
