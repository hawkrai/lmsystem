using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Core.Data;
using LMPlatform.Models.DP;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.Models
{
    public class Materials : ModelBase
    {
        public string Name
        {
            get;
            set;
        }

        public virtual Folders Folders { get; set; }
    }
}