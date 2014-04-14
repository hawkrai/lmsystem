using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class ProjectRole : ModelBase
    {
        [DisplayName("Роль")]
        public string Name
        {
            get;
            set;
        }

        public ICollection<ProjectUser> ProjectUser
        {
            get; 
            set;
        }
    }
}
