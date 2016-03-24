namespace Application.Infrastructure.CTO
{
    public class CourseProjectConsultationMarkData
    {
        public int? Id { get; set; }

        public int ConsultationDateId { get; set; }

        public int StudentId { get; set; }

        public string Mark { get; set; }

        public string Comments { get; set; }
    }
}
