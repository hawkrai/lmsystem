using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Core.Data;
using LMPlatform.Models.DP;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.Models
{
    public class Materials : ModelBase
    {
        [Required, StringLength(128)]
        public string Name
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public virtual Folders Folders { get; set; }
    }
}