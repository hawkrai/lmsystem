namespace LMPlatform.UI.Services.Modules.Messages
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class RecipientsResult : ResultViewData
    {
        [DataMember]
        public List<RecipientViewData> Recipients { get; set; }
    }
}