using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models.KnowledgeTesting;
using LMPlatform.Models.CP;

namespace LMPlatform.Models
{
    public class Subject : ModelBase
    {
        public virtual ICollection<Concept> Concept { get; set; }

        public string Name
        {
            get; 
            set;
        }

        public string ShortName
        {
            get; 
            set;
        }

		public string Color
		{
			get;
			set;
		}

        public bool IsArchive { get; set; }

        public ICollection<SubjectGroup> SubjectGroups
        {
            get; 
            set;
        }

        public ICollection<SubjectLecturer> SubjectLecturers
        {
            get;
            set;
        } 

        public ICollection<Test> SubjectTests
        {
            get;
            set;
        }

        public ICollection<SubjectModule> SubjectModules
        {
            get;
            set;
        }

        public ICollection<SubjectNews> SubjectNewses
        {
            get;
            set;
        }

        public ICollection<Lectures> Lectures
        {
            get; 
            set;
        }

        public ICollection<Labs> Labs
        {
            get;
            set;
        }

        public ICollection<CourseProject> CourseProjects
        {
            get;
            set;
        }

        public ICollection<CourseProjectConsultationDate> CourseProjectsConsultationDates
        {
            get;
            set;
        }

        public virtual ICollection<CoursePercentagesGraph> CoursePersentagesGraphs
        {
            get;
            set;
        }


        public ICollection<Practical> Practicals
        {
            get;
            set;
        }

        public ICollection<LecturesScheduleVisiting> LecturesScheduleVisitings
        {
            get;
            set;
        }

        public ICollection<ScheduleProtectionPractical> ScheduleProtectionPracticals { get; set; }
    }
}