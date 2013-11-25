using Application.Core.Data;

namespace LMPlatform.Models
{
    public class ProjectStudent : ModelBase 
    {
        public int StudentId
        {
            get; 
            set;
        }

        public int Role
        {
            get; 
            set;
        }

        public int ProjectId
        {
            get; 
            set;
        }

        public Student Student
        {
            get; 
            set;
        }

        public Project Project
        {
            get; 
            set;
        }
    }
}
