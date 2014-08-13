using System.ComponentModel.DataAnnotations;

namespace LMPlatform.Models.DP
{
    public class DiplomProjectConsultationMark
    {
        public int Id { get; set; }

        public int ConsultationDateId { get; set; }

        public int StudentId { get; set; }

        [StringLength(1)]
        public string Mark { get; set; }

        [StringLength(50)]
        public string Comments { get; set; }

        public virtual DiplomProjectConsultationDate DiplomProjectConsultationDate { get; set; }

        public Student Student { get; set; }
    }
}
