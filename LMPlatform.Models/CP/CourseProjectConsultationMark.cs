using System.ComponentModel.DataAnnotations;

namespace LMPlatform.Models.CP
{
    public class CourseProjectConsultationMark
    {
        public int Id { get; set; }

        public int ConsultationDateId { get; set; }

        public int StudentId { get; set; }

        //todo: make it as byte
        [StringLength(2)]
        public string Mark { get; set; }

        [StringLength(50)]
        public string Comments { get; set; }

        public virtual CourseProjectConsultationDate CourseProjectConsultationDate { get; set; }

        public Student Student { get; set; }
    }
}
