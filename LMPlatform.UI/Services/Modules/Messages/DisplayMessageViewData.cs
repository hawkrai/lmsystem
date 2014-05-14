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
    public class DisplayMessageViewData : MessagesViewData
    {
        public DisplayMessageViewData(UserMessages userMessage)
            : base(userMessage)
        {
            Body = userMessage.Message.Text;
            Attachments = userMessage.Message.Attachments;
            Date = userMessage.Date.ToString("F", new CultureInfo("ru-RU"));
        }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public IEnumerable<Attachment> Attachments
        {
            get;
            set;
        }
    }
}