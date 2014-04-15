using Application.Core.Data;

namespace LMPlatform.Models
{
    public class ProjectUser : ModelBase 
    {
        public int UserId
        {
            get; 
            set;
        }

        public int ProjectRoleId
        {
            get; 
            set;
        }

        public int ProjectId
        {
            get; 
            set;
        }

        public User User
        {
            get; 
            set;
        }

        public Project Project
        {
            get; 
            set;
        }

        public ProjectRole ProjectRole 
        { 
            get; 
            set; 
        }
    }
}
