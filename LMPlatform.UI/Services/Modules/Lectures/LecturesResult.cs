namespace LMPlatform.UI.Services.Modules.Lectures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class LecturesResult : ResultViewData
    {
        [DataMember]
        public List<LecturesViewData> Lectures { get; set; } 
    }
}