using System;
using System.Collections.Generic;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class ScheduleProtectionPractical : ModelBase
    {
        public int Order { get; set; }

        public int Rating { get; set; }

        public DateTime Date { get; set; }

        public int SuGroupId { get; set; }

        public SubGroup SubGroup { get; set; }

        public int PracticalId { get; set; }

        public Practical Practical { get; set; }

        public ICollection<ScheduleProtectionPracticalMark> ScheduleProtectionPracticalMarks { get; set; }     
    }
}