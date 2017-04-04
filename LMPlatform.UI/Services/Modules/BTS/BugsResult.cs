using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class BugsResult : ResultViewData
    {
        [DataMember]
        public List<BugsViewData> Bugs { get; set; }

        [DataMember]
        public int TotalCount { get; set; }
    }
}