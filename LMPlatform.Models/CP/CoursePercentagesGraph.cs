using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMPlatform.Models.CP
{
    [Table("CoursePercentagesGraph")]
    public class CoursePercentagesGraph
    {
        public CoursePercentagesGraph()
        {
            CoursePercentagesResults = new HashSet<CoursePercentagesResult>();
        }

        public int Id { get; set; }

        public int LecturerId { get; set; }

        public int SubjectId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public double Percentage { get; set; }

        private DateTime _date;

        public DateTime Date
        {
            get
            {
                return _date.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(_date, DateTimeKind.Utc)
                    : _date.ToUniversalTime();
            }

            set
            {
                _date = value;
            }
        }

        public virtual ICollection<CoursePercentagesResult> CoursePercentagesResults { get; set; }

        public virtual ICollection<CoursePercentagesGraphToGroup> CoursePercentagesGraphToGroups { get; set; }

        public virtual Lecturer Lecturer { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
