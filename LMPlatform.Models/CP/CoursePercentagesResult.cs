using System.ComponentModel.DataAnnotations;

namespace LMPlatform.Models.CP
{
    public class CoursePercentagesResult
    {
        public int Id { get; set; }

        public int CoursePercentagesGraphId { get; set; }

        public int StudentId { get; set; }

        public int? Mark { get; set; }

        [StringLength(50)]
        public string Comments { get; set; }

        public virtual CoursePercentagesGraph CoursePercentagesGraph { get; set; }

        public virtual Student Student { get; set; }
    }
}
