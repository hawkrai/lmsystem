using System.Collections.Generic;
using System.ComponentModel;
using Application.Core.Data;

namespace LMPlatform.Models.BTS
{
    public class BugSymptom : ModelBase 
    {
        [DisplayName("Симптом")]
        public string Name { get; set; }

        public ICollection<Bug> Bug { get; set; }
    }
}
