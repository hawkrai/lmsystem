using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class StudentGroupUser : ModelBase
    {
        public int Number { get; set; }

        public string Name { get; set; }

        public ICollection<string> ProjectCreatorName { get; set; } 

        public ICollection<string> ProjectName { get; set; }
 
        public ICollection<string> ProjectRole { get; set; } 
    }
}
