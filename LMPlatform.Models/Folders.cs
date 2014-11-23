using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Core.Data;
using LMPlatform.Models.DP;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.Models
{
    public class Folders : ModelBase
    {
        public string Name
        {
            get;
            set;
        }

        public int Pid
        {
            get;
            set;
        }

        public virtual ICollection<Materials> Materials { get; set; }
    }
}