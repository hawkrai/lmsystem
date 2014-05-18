using System.Collections.Generic;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class SubGroup : ModelBase
    {
        public string Name
        {
            get;
            set;
        }

        public int SubjectGroupId
        {
            get;
            set;
        }

        public SubjectGroup SubjectGroup
        {
            get;
            set;
        }

        public ICollection<SubjectStudent> SubjectStudents
        {
            get;
            set;
        }

        public ICollection<ScheduleProtectionLabs> ScheduleProtectionLabs
        {
            get;
            set;
        } 
    }
}