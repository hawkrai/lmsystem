namespace LMPlatform.UI.Services.Modules.Lectures
{
    using System.Runtime.Serialization;

    using LMPlatform.Models;

    [DataContract]
    public class AttachmentViewData
    {
        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public string FileName
        {
            get;
            set;
        }

        [DataMember]
        public string PathName
        {
            get;
            set;
        }

        [DataMember]
        public AttachmentType AttachmentType
        {
            get;
            set;
        } 
    }
}