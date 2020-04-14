using LMPlatform.UI.Services.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

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