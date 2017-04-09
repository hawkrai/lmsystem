using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class UserProjectParticipationsResult : ResultViewData
    {
        [DataMember]
        public List<UserProjectParticipationViewData> Projects { get; set; }

        [DataMember]
        public int TotalCount { get; set; }
    }
}