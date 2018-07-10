using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectMatrixResult : ResultViewData
    {
        [DataMember]
        public ProjectMatrixViewData Project { get; set; }
    }
}