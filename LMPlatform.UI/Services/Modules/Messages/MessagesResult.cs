namespace LMPlatform.UI.Services.Modules.Messages
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class MessagesResult : ResultViewData
    {
        [DataMember]
        public List<MessagesViewData> Messages { get; set; } 
    }
}