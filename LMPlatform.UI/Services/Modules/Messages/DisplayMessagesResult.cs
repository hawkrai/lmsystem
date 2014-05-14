namespace LMPlatform.UI.Services.Modules.Messages
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DisplayMessageResult : ResultViewData
    {
        [DataMember]
        public DisplayMessageViewData DisplayMessage { get; set; }
    }
}