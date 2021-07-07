using System.Runtime.Serialization;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectFileViewData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string AttachmentType { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string PathName { get; set; }

        public ProjectFileViewData(Attachment attachment)
        {
            Id = attachment.Id;
            Name = attachment.Name;
            FileName = attachment.FileName;
            PathName = attachment.PathName;
            AttachmentType = attachment.AttachmentType.ToString();
        }
    }
}