using Application.Core.Data;

namespace LMPlatform.Models
{
    public class ProjectUser : ModelBase 
    {
        public User UserID
        {
            get; 
            set;
        }

        public int Role
        {
            get; 
            set;
        }

        public Project ProjectID
        {
            get; 
            set;
        }
    }
}
