using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Application.Core.Constants;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.MessageManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.Models;
using LMPlatform.UI.ViewModels.MessageViewModels;
using Mvc.JQuery.Datatables;
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

            msg.Attachment = attachments;

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

        [HttpPost]
        public DataTablesResult<DisplayMessageViewModel> GetCollectionMessages(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            bool? incoming = null;
            try
            {
                incoming = bool.Parse(Request.QueryString["isIncoming"]);
            }
            catch (ArgumentNullException)
            {
            }
            catch (FormatException)
            {
            }

            var messages = MessageManagementService.GetUserMessagesPageable(WebSecurity.CurrentUserId, incoming, pageInfo: dataTableParam.ToPageInfo(), searchString: searchString);
            return DataTableExtensions.GetResults(messages.Items.Select(m => DisplayMessageViewModel.FormMessageToDisplay(m, PartialViewToString("_EditGlyphLinks", m.Id))), dataTableParam, messages.TotalCount);
        }

        public JsonResult GetSelectListOptions(string term)
        {
            var recip = MessageManagementService.GetRecipientsList(WebSecurity.CurrentUserId);

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
