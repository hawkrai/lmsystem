using System.Collections.Generic;
using System.Runtime.Serialization;
using LMPlatform.UI.Services.Modules.Lectures;

namespace LMPlatform.UI.Services.Modules.Labs
{
    [DataContract]
    public class UserlabFilesViewData
    {
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public List<AttachmentViewData> Attachments { get; set; } 
    }
}