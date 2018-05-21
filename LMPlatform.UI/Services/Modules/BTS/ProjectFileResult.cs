using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectFileResult : ResultViewData
    {
        [DataMember]
        public ProjectFileViewData ProjectFile { get; set; }
    }
}