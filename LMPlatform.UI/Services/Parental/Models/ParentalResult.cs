using LMPlatform.UI.Services.Modules;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Parental.Models
{
    [DataContract]
    public class ParentalResult : ResultViewData
    {
        [DataMember]
        public List<ParentalUser> Students { get; set; }

        [DataMember]
        public string GroupName { get; set; }
    }
}