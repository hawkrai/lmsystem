using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectFilesResult : ResultViewData
    {
        [DataMember]
        public List<ProjectFileViewData> ProjectFiles { get; set; }
    }
}