using Application.Core.Data;

namespace LMPlatform.Models
{
    public class LecturesVisitMark : ModelBase
    {
        public int StudentId { get; set; }

        public string Mark { get; set; }

        public int LecturesScheduleVisitingId { get; set; }
        
        public Student Student { get; set; }

        public LecturesScheduleVisiting LecturesScheduleVisiting { get; set; }
    }
}