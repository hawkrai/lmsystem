namespace LMPlatform.UI.Services.Modules.Messages
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using LMPlatform.Models;

    [DataContract]
    public class AttachmentResult : ResultViewData
    {
        [DataMember]
        public List<Attachment> Files { get; set; }

        [DataMember]
        public string ServerPath { get; set; }
    }
}