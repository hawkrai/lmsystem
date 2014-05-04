using System.ComponentModel.DataAnnotations;

namespace LMPlatform.Models.DP
{
    public class DiplomPercentagesResult
    {
        public int Id { get; set; }

        public int DiplomPercentagesGraphId { get; set; }

        public int StudentId { get; set; }

        public int? Mark { get; set; }

        [StringLength(50)]
        public string Comments { get; set; }

        public virtual DiplomPercentagesGraph DiplomPercentagesGraph { get; set; }
    }
}
