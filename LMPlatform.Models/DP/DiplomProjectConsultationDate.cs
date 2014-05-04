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

        public DateTime Day { get; set; }

        public virtual ICollection<DiplomProjectConsultationMark> DiplomProjectConsultationMarks { get; set; }
    }
}
