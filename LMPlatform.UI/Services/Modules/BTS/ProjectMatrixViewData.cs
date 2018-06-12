using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectMatrixViewData
    {
        [DataMember]
        public string RequirementsFileName { get; set; }

        [DataMember]
        public string TestsFileName { get; set; }
    }
}