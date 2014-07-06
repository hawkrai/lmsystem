using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMPlatform.Models.DP
{
    [Table("DiplomPercentagesGraph")]
    public class DiplomPercentagesGraph
    {
        public DiplomPercentagesGraph()
        {
            DiplomPercentagesResults = new HashSet<DiplomPercentagesResult>();
        }

        public int Id { get; set; }

        public int LecturerId { get; set; }

        public int GroupId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public double Percentage { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<DiplomPercentagesResult> DiplomPercentagesResults { get; set; }

        public virtual ICollection<DiplomPercentagesGraphToGroup> DiplomPercentagesGraphToGroups { get; set; }
    }
}
