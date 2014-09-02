using System.Collections.Generic;
using System.Runtime.Serialization;
using LMPlatform.Models;
using LMPlatform.UI.Services.Modules.Lectures;

namespace LMPlatform.UI.Services.Modules.Labs
{
    [DataContract]
    public class UserlabFilesViewData
    {
		[DataMember]
		public int Id { get; set; }
        [DataMember]
        public string Comments { get; set; }
		[DataMember]
		public string PathFile { get; set; }
        [DataMember]
        public List<Attachment> Attachments { get; set; } 
    }
}