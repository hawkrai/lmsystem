using LMPlatform.Models.DP;

namespace LMPlatform.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Application.Core.Data;

    public class Group : ModelBase
    {
        public Group()
        {
            Students = new Collection<Student>();
        }

        public string Name
        {
            get;
            set;
        }

        public string StartYear
        {
            get;
            set;
        }

        public string GraduationYear
        {
            get;
            set;
        }

        public ICollection<Student> Students
        {
            get;
            set;
        }

        public ICollection<SubjectGroup> SubjectGroups
        {
            get; 
            set;
        }

        public ICollection<ScheduleProtectionPractical> ScheduleProtectionPracticals { get; set; }

        public virtual ICollection<DiplomProjectGroup> DiplomProjectGroups { get; set; }
    }
}