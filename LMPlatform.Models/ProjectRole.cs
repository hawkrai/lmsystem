using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class ProjectRole : ModelBase
    {
        public string Name
        {
            get;
            set;
        }

        public ProjectUser ProjectUser
        {
            get; 
            set;
        }
    }
}
