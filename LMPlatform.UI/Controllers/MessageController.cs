using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using LMPlatform.UI.ViewModels;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    public class MessageController : BasicController
    {
        public ActionResult WriteMessage(int? id)
        {
            var messageViewModel = new MessageViewModel()
            {
                FromId = WebSecurity.CurrentUserId
            };
            return PartialView("Common/_MessageForm", messageViewModel);
        }

        [HttpPost]
        public ActionResult WriteMessage(MessageViewModel msg)
        {
            msg.SaveMessage();

            if (TempData["Action"] != null && TempData["Controller"] != null)
            {
                var action = TempData["Action"].ToString();
                var controller = TempData["Controller"].ToString();
                return RedirectToAction(action, controller);
            }

            return Request.UrlReferrer != null ? Redirect(Request.UrlReferrer.AbsolutePath) : null;
        }
    }
}
