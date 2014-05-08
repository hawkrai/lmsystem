namespace LMPlatform.UI.Services.Modules.Messages
{
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
            AthorName = userMessage.Author.FullName;
            Subject = userMessage.Message.Subject;
            Body = userMessage.Message.Text;
            IsRead = userMessage.IsRead;
        }

        [DataMember]
        public string AthorName { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public bool IsRead { get; set; }
    }
}