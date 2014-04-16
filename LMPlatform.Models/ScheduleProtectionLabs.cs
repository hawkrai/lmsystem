namespace LMPlatform.Models
{
    using System;

    using Application.Core.Data;

    public class ScheduleProtectionLabs : ModelBase
    {
        public int Order { get; set; }

        public int Rating { get; set; }

        public DateTime Date { get; set; }

        public int SuGroupId { get; set; }

        public SubGroup SubGroup { get; set; }

        public int LabsId { get; set; }

        public Labs Labs { get; set; }
    }
}