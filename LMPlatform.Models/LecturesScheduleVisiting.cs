using System.Collections.Generic;

namespace LMPlatform.Models
{
    using System;

    using Application.Core.Data;

    public class LecturesScheduleVisiting : ModelBase
    {
        public DateTime Date { get; set; }

        public int SubjectId { get; set; }

        public Subject Subject { get; set; }

        public ICollection<LecturesVisitMark> LecturesVisitMarks { get; set; } 
    }
}