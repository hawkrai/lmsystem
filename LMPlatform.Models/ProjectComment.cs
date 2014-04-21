using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class ProjectComment : ModelBase
    {
        public string CommentText { get; set; }

        public int UserId { get; set; }

        public DateTime CommentingDate { get; set; }

        public int ProjectId { get; set; }

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
    }
}
