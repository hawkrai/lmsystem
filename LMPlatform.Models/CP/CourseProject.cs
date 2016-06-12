using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMPlatform.Models.CP
{
    public class CourseProject
    {
        public int CourseProjectId { get; set; }

        public CourseProject()
        {
            AssignedCourseProjects = new HashSet<AssignedCourseProject>();
            CourseProjectGroups = new HashSet<CourseProjectGroup>();
        }

        [Required]
        [StringLength(2048)]
        public string Theme { get; set; }

        public int? LecturerId { get; set; }

        public string InputData { get; set; }

        public string Univer { get; set; }

        public string Faculty { get; set; }

        public string HeadCathedra { get; set; }

        public string RpzContent { get; set; }

        public string DrawMaterials { get; set; }

        public string Consultants { get; set; }

        public string Workflow { get; set; }

        public DateTime? DateEnd { get; set; }

        public DateTime? DateStart { get; set; }

        public int? SubjectId
        {
            get;
            set;
        }

        public virtual Subject Subject
        {
            get;
            set;
        }

        public virtual Lecturer Lecturer { get; set; }

        public virtual ICollection<AssignedCourseProject> AssignedCourseProjects { get; set; }

        public virtual ICollection<CourseProjectGroup> CourseProjectGroups { get; set; }
    }
}
