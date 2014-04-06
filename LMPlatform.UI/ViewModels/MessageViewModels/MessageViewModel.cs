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

        [Display(Name = "Сообщение")]
        [Required(ErrorMessage = "Введите текст сообщения")]
        [DataType(DataType.MultilineText)]
        public string MessageText { get; set; }

        [Display(Name = "Тема")]
        [Required(ErrorMessage = "Введите тему сообщения")]
        [DataType(DataType.Text), MaxLength(50, ErrorMessage = "Длина поля Тема не должна превышать 50 символов")]
        public string Subject { get; set; }

        public List<UserMessages> UserMessages { get; set; }

        public List<Attachment> Attachment { get; set; }

        public IList<SelectListItem> GetRecipientsSelectList()
        {
            var recip = MessageManagementService.GetRecipients(FromId);

            return recip.Select(r => new SelectListItem
                {
                    Text = r.FullName,
                    Value = r.Id.ToString(CultureInfo.InvariantCulture)
                }).ToList();
        }

        public void SaveMessage()
        {
            var msg = new Message(MessageText, Subject);

            if (Attachment != null && Attachment.Any())
            {
                msg.AttachmentsPath = Guid.NewGuid();
                Attachment.ForEach(a => a.PathName = msg.AttachmentsPath.ToString());
                msg.Attachments = Attachment;
            }

            MessageManagementService.SaveMessage(msg);

            foreach (var recipient in Recipients)
            {
                var userMsg = new UserMessages(recipient, FromId, msg.Id);
                MessageManagementService.SaveUserMessages(userMsg);
            }
        }
    }
}