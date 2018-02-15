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
        public const int Developer = 1;
        public const int Tester = 2;
        public const int Leader = 3;

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
