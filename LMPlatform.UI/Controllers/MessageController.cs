using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Application.Core.Constants;
using Application.Core.UI.Controllers;
using Application.Infrastructure.MessageManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.Models;
using LMPlatform.UI.ViewModels.MessageViewModels;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    [Authorize]
    public class MessageController : BasicController
    {
        public ActionResult Index()
        {
            var model = new MessageListViewModel();

            if (User.IsInRole(Constants.Roles.Admin))
            {
                return View("Messages", "Layouts/_AdministrationLayout", model);
            }

            return View("Messages", "Layouts/_MainUsingNavLayout", model);
        }

        public ActionResult WriteMessage(int? id)
        {
            var messageViewModel = new MessageViewModel
                {
                    FromId = WebSecurity.CurrentUserId,
                    Attachment = new List<Attachment>()
                };

            return PartialView("Common/_MessageForm", messageViewModel);
        }

        [HttpPost]
        public ActionResult WriteMessage(MessageViewModel msg, string itemAttachments)
        {
            var jsonSerializer = new JavaScriptSerializer();
            var attachments = jsonSerializer.Deserialize<List<Attachment>>(itemAttachments);

            if (ModelState.IsValid && msg.FromId == WebSecurity.CurrentUserId)
            {
                msg.SaveMessage();
            }

            return RedirectToAction("Index");
        }

        public int MessagesCount()
        {
            var messagesCount = MessageManagementService.GetUnreadUserMessages(WebSecurity.CurrentUserId).Count();
            return messagesCount;
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
