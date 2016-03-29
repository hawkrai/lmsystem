using System;

namespace Application.Infrastructure.CTO
{
    public class CourseProjectConsultationDateData
    {
        public int? Id { get; set; }

        public int LecturerId { get; set; }

        public DateTime Day { get; set; }

        public int SubjectId { get; set; }
    }
}
