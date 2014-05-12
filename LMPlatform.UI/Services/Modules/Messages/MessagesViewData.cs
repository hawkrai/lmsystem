namespace LMPlatform.UI.Services.Modules.Messages
{
    using System;
    using System.Collections.Generic;
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
            Body = userMessage.Message.Text;
            PreviewText = !string.IsNullOrEmpty(Body) ? Body.Substring(0, Math.Min(Body.Length, 100)) : Body;
            IsRead = userMessage.IsRead;
            Attachments = userMessage.Message.Attachments;
            Date = userMessage.Date.ToString(userMessage.Date.Date == DateTime.Now.Date ? "t" : "d");
        }

        [DataMember]
        public string AthorName { get; set; }

        [DataMember]
        public string AthorId { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public string PreviewText { get; set; }

        [DataMember]
        public bool IsRead { get; set; }

        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public IEnumerable<Attachment> Attachments
        {
            get;
            set;
        } 
    }
}