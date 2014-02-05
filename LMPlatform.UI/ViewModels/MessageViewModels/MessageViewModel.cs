using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Application.Core;
using Application.Infrastructure.MessageManagement;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.MessageViewModels
{
    public class MessageViewModel
    {
        private readonly LazyDependency<IMessageManagementService> _messageManagementService =
            new LazyDependency<IMessageManagementService>();

        public IMessageManagementService MessageManagementService
        {
            get { return _messageManagementService.Value; }
        }

        [HiddenInput(DisplayValue = false)]
        public int FromId { get; set; }

        [Required(ErrorMessage = "Необходимо указать получателя")]
        [Display(Name = "Кому")]
        public List<int> Recipients { get; set; }

        [Required(ErrorMessage = "Введите текст сообщения")]
        [Display(Name = "Сообщение")]
        [DataType(DataType.MultilineText)]
        public string MessageText { get; set; }

        public List<UserMessages> UserMessages { get; set; }

        public List<Attachment> Attachment { get; set; }

        public IList<SelectListItem> GetRecipientsSelectList()
        {
            var recip = MessageManagementService.GetRecipientsList(FromId);

            return recip.Select(r => new SelectListItem
                {
                    Text = r.FullName,
                    Value = r.Id.ToString(CultureInfo.InvariantCulture)
                }).ToList();
        }

        public void SaveMessage()
        {
            var msg = new Message(MessageText);
            MessageManagementService.SaveMessage(msg);

            foreach (var recipient in Recipients)
            {
                var userMsg = new UserMessages(recipient, FromId, msg.Id);
                MessageManagementService.SaveUserMessages(userMsg);
            }
        }
    }
}