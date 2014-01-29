using System.Linq;
using System.Web.Mvc;
using Application.Core.Constants;
using Application.Core.UI.Controllers;
using Application.Infrastructure.MessageManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.UI.ViewModels.MessageViewModels;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    [Authorize]
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

        public ActionResult WriteMessage(int? id)
        {
            var messageViewModel = new MessageViewModel
                {
                    FromId = WebSecurity.CurrentUserId
                };

            return PartialView("Common/_MessageForm", messageViewModel);
        }

        [HttpPost]
        public ActionResult WriteMessage(MessageViewModel msg)
        {
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
