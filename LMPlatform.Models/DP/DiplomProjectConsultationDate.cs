using System;
using System.Collections.Generic;

namespace LMPlatform.Models.DP
{
    public class DiplomProjectConsultationDate
    {
        public DiplomProjectConsultationDate()
        {
            DiplomProjectConsultationMarks = new HashSet<DiplomProjectConsultationMark>();
        }

        public int Id { get; set; }

        public int LecturerId { get; set; }

        private DateTime _day;

        public DateTime Day
        {
            get
            {
                return _day.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(_day, DateTimeKind.Utc)
                    : _day.ToUniversalTime();
            }

            set
            {
                _day = value;
            }
        }

        public virtual ICollection<DiplomProjectConsultationMark> DiplomProjectConsultationMarks { get; set; }
    }
}
