namespace LMPlatform.UI.Services.Modules.Materials
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class DocumentsResult : ResultViewData
    {
        [DataMember]
        public DocumentsViewData Document { get; set; }
 
        [DataMember]
        public List<DocumentsViewData> Documents { get; set; } 
    }
}