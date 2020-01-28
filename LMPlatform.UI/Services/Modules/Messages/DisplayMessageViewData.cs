using System.Globalization;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.Messages
{
	using Models;

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