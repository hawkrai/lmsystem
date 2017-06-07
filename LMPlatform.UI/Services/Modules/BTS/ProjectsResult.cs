using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectsResult : ResultViewData
    {
        [DataMember]
        public List<ProjectViewData> Projects { get; set; }

        [DataMember]
        public int TotalCount { get; set; }
    }
}