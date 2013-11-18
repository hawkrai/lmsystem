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
    }
}