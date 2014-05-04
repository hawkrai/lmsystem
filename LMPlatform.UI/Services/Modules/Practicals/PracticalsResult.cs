namespace LMPlatform.UI.Services.Modules.Practicals
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class PracticalsResult : ResultViewData
    {
        [DataMember]
        public List<PracticalsViewData> Practicals { get; set; } 
    }
}