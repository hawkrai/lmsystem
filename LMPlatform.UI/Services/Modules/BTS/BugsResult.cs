using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class BugsResult : ResultViewData
    {
        [DataMember]
        public List<BugViewData> Bugs { get; set; }

        [DataMember]
        public int TotalCount { get; set; }
    }
}