using Application.Core.Data;

namespace LMPlatform.Models
{
    public class SubjectStudent : ModelBase
    {
        public int StudentId
        {
            get;
            set;
        }

        public int SubGroupsId
        {
            get;
            set;
        }

        public int SubjectGroupId
        {
            get;
            set;
        }

        public Student Student
        {
            get;
            set;
        }

        public SubGroups SubGroups
        {
            get;
            set;
        }

        public SubjectGroup SubjectGroup
        {
            get;
            set;
        }
    }
}