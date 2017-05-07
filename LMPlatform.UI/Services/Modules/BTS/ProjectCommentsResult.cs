using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectCommentsResult : ResultViewData
    {
        [DataMember]
        public List<ProjectCommentViewData> Comments { get; set; }
    }
}