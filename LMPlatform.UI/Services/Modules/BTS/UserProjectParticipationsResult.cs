using System.Collections.Generic;
using System.Runtime.Serialization;

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