using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectCommentsResult : ResultViewData
    {
        [DataMember]
        public List<ProjectCommentViewData> Comments { get; set; }
    }
}