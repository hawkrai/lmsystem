using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Application.Core.Constants;
using Application.Core.Extensions;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.MessageManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.Models;
using LMPlatform.UI.Attributes;
using LMPlatform.UI.ViewModels.MessageViewModels;
using Mvc.JQuery.Datatables;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    [JwtAuth]
    public class MessageController : BasicController
    {
        public ActionResult Index()
        {
            if (User.IsInRole(Constants.Roles.Admin))
            {
                return View("Messages", "Layouts/_AdministrationLayout");
            }

            return View("Messages", "Layouts/_MainUsingNavLayout");
        }

        public ActionResult WriteMessage(int? id, bool toadmin = false, string resubject = "")
        {
            var messageViewModel = new MessageViewModel(toadmin)
                {
                    FromId = WebSecurity.CurrentUserId,
                    Attachment = new List<Attachment>(),
                    Subject = string.IsNullOrEmpty(resubject) ? resubject : "Re:" + resubject,
                };

            if (id.HasValue)
            {
                messageViewModel.Recipients = new List<int>() { id.Value };
            }

            return PartialView("Common/_MessageForm", messageViewModel);
        }

        public ActionResult DisplayMessage()
        {
            return PartialView("Messages/_DisplayMessage");
        }

        public ActionResult MessageForm()
        {
            return PartialView("Messages/_MessageForm");
        }

        [HttpPost]
        public ActionResult WriteMessage(MessageViewModel msg, string itemAttachments)
        {
            var jsonSerializer = new JavaScriptSerializer();
            var attachments = jsonSerializer.Deserialize<List<Attachment>>(itemAttachments);

            msg.Attachment = attachments;

            if (ModelState.IsValid && msg.FromId == WebSecurity.CurrentUserId)
            {
                msg.SaveMessage();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult WriteMessageAjax(MessageViewModel msg, string itemAttachments)
        {
            if (ModelState.IsValid && msg.FromId == WebSecurity.CurrentUserId)
            {
                try
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    var attachments = jsonSerializer.Deserialize<List<Attachment>>(itemAttachments);

                    msg.Attachment = attachments;

                    msg.SaveMessage();
                    return Json(new { code = 200, resultMessage = "Сообщение отправлено" });
                }
                catch
                {
                    return Json(new { code = 500, resultMessage = "При сохранении сообщения произошла ошибка" });
                }
            }

            msg.Attachment = new List<Attachment>();

            return PartialView("Common/_MessageForm", msg);
        }

        public int MessagesCount()
        {
            var messagesCount = MessageManagementService.GetUnreadUserMessages(WebSecurity.CurrentUserId).Count();
            return messagesCount;
        }

        public JsonResult GetSelectListOptions(string term)
        {
            var recip = MessageManagementService.GetRecipients(WebSecurity.CurrentUserId);

            var result = recip.Where(r => r.FullName.ToLower().Contains(term.ToLower()))
                .Select(r => new
                {
                    text = r.FullName,
                    value = r.Id.ToString()
                }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public IUsersManagementService UsersManagementService
        {
            get
            {
                return ApplicationService<IUsersManagementService>();
            }
        }

        public IMessageManagementService MessageManagementService
        {
            get
            {
                return ApplicationService<IMessageManagementService>();
            }
        }
    }
}
