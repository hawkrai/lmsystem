using System.Collections.Generic;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class Module : ModelBase
    {
        public string Name
        {
            get; 
            set;
        }

        public string DisplayName
        {
            get; 
            set;
        }

        public bool Visible { get; set; }

        public ModuleType ModuleType
        {
            get;
            set;
        }

        public ICollection<SubjectModule> SubjectModules
        {
            get; 
            set;
        }
    }
}