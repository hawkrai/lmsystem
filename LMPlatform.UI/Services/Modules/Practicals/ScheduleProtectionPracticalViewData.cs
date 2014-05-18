using System;
using System.Runtime.Serialization;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.Practicals
{
    [DataContract]
    public class ScheduleProtectionPracticalViewData
    {
        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public int GroupId { get; set; }

        [DataMember]
        public int SubjectId { get; set; }

        [DataMember]
        public int ScheduleProtectionPracticalId { get; set; }
    }
}