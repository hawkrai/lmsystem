using System.Collections.Generic;
using System.ComponentModel;
using Application.Core.Data;

namespace LMPlatform.Models.BTS
{
    public class ProjectMatrixRequirement : ModelBase
    {
        public string Name { get; set; }

        public bool Covered { get; set; }

        public Project Project { get; set; }

        public int ProjectId { get; set; }
    }
}
