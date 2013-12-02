using System.ComponentModel;
using Application.Core.Data;

namespace LMPlatform.Models.BTS
{
    public class BugSeverity : ModelBase
    {
        [DisplayName("Важность")]
        public string Name { get; set; }

        public Bug Bug
        {
            get; 
            set;
        }
    }
}
