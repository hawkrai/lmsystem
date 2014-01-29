using System.Collections.Generic;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class SubjectGroup : ModelBase
    {
        public int GroupId
        {
            get; 
            set;
        }

        public int SubjectId
        {
            get; 
            set;
        }

        public Group Group
        {
            get; 
            set;
        }

        public Subject Subject
        {
            get; 
            set;
        }

        public ICollection<SubGroups> SubGroupses
        {
            get;
            set;
        }

        public ICollection<SubjectStudent> SubjectStudents
        {
            get;
            set;
        }
    }
}