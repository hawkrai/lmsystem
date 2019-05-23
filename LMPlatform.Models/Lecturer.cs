using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Core.Data;
using LMPlatform.Models.DP;
using LMPlatform.Models.CP;

namespace LMPlatform.Models
{
    public class Lecturer : ModelBase
    {
        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string MiddleName
        {
            get;
            set;
        }

        public User User
        {
            get;
            set;
        }

        [NotMapped]
        public string FullName
        {
            get { return string.Format("{0} {1} {2}", LastName, FirstName, MiddleName); }
        }

        public ICollection<SubjectLecturer> SubjectLecturers
        {
            get;
            set;
        }

        public virtual ICollection<DiplomProject> DiplomProjects
        {
            get;
            set;
        }

        public virtual ICollection<DiplomPercentagesGraph> DiplomPercentagesGraphs
        {
            get;
            set;
        }

        public virtual ICollection<DiplomProjectConsultationDate> DiplomProjectConsultationDates
        {
            get;
            set;
        }



        public virtual ICollection<CourseProject> CourseProjects
        {
            get;
            set;
        }

        public virtual ICollection<CoursePercentagesGraph> CoursePercentagesGraphs
        {
            get;
            set;
        }

        public virtual ICollection<CourseProjectConsultationDate> CourseProjectConsultationDates
        {
            get;
            set;
        }

        public virtual ICollection<Group> SecretaryGroups
        {
            get;
            set;
        }

        public string Skill { get; set; }
		
        public bool IsSecretary { get; set; }
        public bool IsActive { get; set; }

        public bool IsLecturerHasGraduateStudents { get; set; }
    }
}