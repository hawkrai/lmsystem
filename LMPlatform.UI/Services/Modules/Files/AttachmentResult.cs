using System.Collections.Generic;
using System.Runtime.Serialization;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.Files
{
	[DataContract]
    public class AttachmentResult : ResultViewData
    {
        [DataMember]
        public List<Attachment> Files { get; set; }

        [DataMember]
        public string ServerPath { get; set; }
    }
}