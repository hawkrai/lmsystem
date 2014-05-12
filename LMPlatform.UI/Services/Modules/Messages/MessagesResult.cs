namespace LMPlatform.UI.Services.Modules.Messages
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class MessagesResult : ResultViewData
    {
        [DataMember]
        public List<MessagesViewData> InboxMessages { get; set; }

        [DataMember]
        public List<MessagesViewData> OutboxMessages { get; set; } 
    }
}