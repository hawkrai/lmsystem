using System.Globalization;

namespace LMPlatform.UI.Services.Modules.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel.Description;

    using Application.Core;
    using Application.Infrastructure.FilesManagement;
    using Application.Infrastructure.MessageManagement;

    using LMPlatform.Models;

    [DataContract]
    public class MessagesViewData
    {
        private readonly LazyDependency<IMessageManagementService> messageManagementService = new LazyDependency<IMessageManagementService>();
        private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get
            {
                return filesManagementService.Value;
            }
        }

        public IMessageManagementService MessageManagementService
        {
            get
            {
                return messageManagementService.Value;
            }
        }

        public MessagesViewData(UserMessages userMessage)
        {
            Id = userMessage.MessageId;
            AthorName = userMessage.Author.FullName;
            AthorId = userMessage.Author.Id.ToString();
            Subject = userMessage.Message.Subject;
            PreviewText = !string.IsNullOrEmpty(userMessage.Message.Text)
                ? userMessage.Message.Text.Substring(0, Math.Min(userMessage.Message.Text.Length, 100))
                : userMessage.Message.Text;
            IsRead = userMessage.IsRead;
            Date = userMessage.Date.ToString(userMessage.Date.Date == DateTime.Now.Date ? "t" : "d", new CultureInfo("ru-RU"));
            Recipients = MessageManagementService.GetMessageRecipients(userMessage.MessageId)
                .Select(e => string.IsNullOrEmpty(e.FullName) ? e.UserName : e.FullName);
            AttachmentsCount = userMessage.Message.Attachments.Any() ? userMessage.Message.Attachments.Count : 0;
        }

        [DataMember]
        public string AthorName { get; set; }

        [DataMember]
        public string AthorId { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public string PreviewText { get; set; }

        [DataMember]
        public bool IsRead { get; set; }

        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int AttachmentsCount { get; set; }

        [DataMember]
        public IEnumerable<string> Recipients
        {
            get;
            set;
        }
    }
}