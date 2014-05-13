using System.Collections.Generic;

namespace LMPlatform.Models
{
    using System;

    using Application.Core.Data;

    public class ScheduleProtectionLabs : ModelBase
    {
        public DateTime Date { get; set; }

        public int SuGroupId { get; set; }

        public SubGroup SubGroup { get; set; }

        public ICollection<ScheduleProtectionLabMark> ScheduleProtectionLabMarks { get; set; }
    }
}