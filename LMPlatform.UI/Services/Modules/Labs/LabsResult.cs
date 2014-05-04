namespace LMPlatform.UI.Services.Modules.Labs
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class LabsResult : ResultViewData
    {
        [DataMember]
        public List<LabsViewData> Labs { get; set; } 
    }
}