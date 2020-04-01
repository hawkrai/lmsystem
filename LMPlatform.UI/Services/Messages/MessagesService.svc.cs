using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Infrastructure.MessageManagement;
using LMPlatform.Models;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.Messages;
using Newtonsoft.Json;
using Application.Core.Extensions;
using WebMatrix.WebData;

namespace LMPlatform.UI.Services.Messages
{
    public class MessagesService : IMessagesService
    {
        private readonly LazyDependency<IMessageManagementService> _messageManagementService =
            new LazyDependency<IMessageManagementService>();

        public IMessageManagementService MessageManagementService => _messageManagementService.Value;

        public MessagesResult GetMessages()
        {
            try
            {
                var userId = WebSecurity.CurrentUserId;
                var model = MessageManagementService.GetUserMessages(userId)
	                .DistinctBy(m => m.MessageId)
	                .ToList();
                var result = new MessagesResult
                    {
						InboxMessages = model.Where(m => m.AuthorId != userId)
							.OrderByDescending(e => e.Date)
							.Select(e => new MessagesViewData(e))
							.ToList(),
						OutboxMessages = model.Where(m => m.AuthorId == userId)
							.OrderByDescending(e => e.Date)
							.Select(e => new MessagesViewData(e))
							.ToList(),
                        Message = "Сообщения успешно загружены",
                        Code = "200"
                    };

                return result;
            }
            catch
            {
				return new MessagesResult
				{
					Message = "Произошла ошибка при получении сообщений",
					Code = "500"
				};
            }
        }

        public DisplayMessageResult GetMessage(string id)
        {
            try
            {
                var userId = WebSecurity.CurrentUserId;
                var msgId = int.Parse(id);

                var msg = MessageManagementService.GetUserMessage(msgId, userId);

                if (msg.RecipientId == userId && !msg.IsRead)
                {
                    MessageManagementService.SetRead(msg.Id);
                }

                return new DisplayMessageResult
                {
                    DisplayMessage = new DisplayMessageViewData(msg),
                    Message = "Сообщение успешно загружено",
                    Code = "200"
                };
            }
            catch
            {
                return new DisplayMessageResult
                {
                    Message = "Произошла ошибка при получении сообщения",
                    Code = "500"
                };
            }
        }

		public RecipientsResult GetRecipients()
		{
			return new RecipientsResult
			{
				Recipients = new List<RecipientViewData>
				{
					new RecipientViewData(0, "Jack"),
					new RecipientViewData(1, "Mike")
				},
				Message = "Успешно",
				Code = "200"
			};
		}

        public ResultViewData Save(string subject, string body, string recipients, string attachments)
        {
            try
            {
                var attachmentsList = JsonConvert.DeserializeObject<List<Attachment>>(attachments).ToList();
                var recipientsList = JsonConvert.DeserializeObject<List<int>>(recipients).ToList();
                var fromId = WebSecurity.CurrentUserId;

                if (!string.IsNullOrEmpty(subject) && subject.Length > 50)
                {
                    subject = subject.Substring(0, 50);
                }

                var msg = new Message(body, subject);

                if (attachmentsList.Any())
                {
                    msg.AttachmentsPath = Guid.NewGuid();
                    attachmentsList.ForEach(a => a.PathName = msg.AttachmentsPath.ToString());
                    msg.Attachments = attachmentsList;
                }

                MessageManagementService.SaveMessage(msg);

                ////if (ToAdmin)
                ////{
                ////    var admin = UserManagementService.GetAdmin();
                ////    var userMsg = new UserMessages(admin.Id, FromId, msg.Id);
                ////    MessageManagementService.SaveUserMessages(userMsg);
                ////}
                ////else
                ////{
                var userMessages = recipientsList.Select(recipientId => new UserMessages(recipientId, fromId, msg.Id));
                foreach (var userMsg in userMessages)
                {
	                MessageManagementService.SaveUserMessages(userMsg);
                }
                ////}

                return new ResultViewData
                {
                    Message = "Сообщение отправлено",
                    Code = "200"
                };
            }
            catch
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при сохранение сообщения",
                    Code = "500"
                };
            }
        }

        public ResultViewData Delete(int messageId)
        {
            try
            {
                var userId = WebSecurity.CurrentUserId;
                var result = MessageManagementService.DeleteMessage(messageId, userId);

                return new ResultViewData
                {
                    Message = "Удаление прошло успешно",
                    Code = "200"
                };
            }
            catch
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при удалении",
                    Code = "500"
                };
            }
        }
    }
}