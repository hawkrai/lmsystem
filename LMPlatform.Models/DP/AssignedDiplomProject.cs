using System;

namespace LMPlatform.Models.DP
{
    public class AssignedDiplomProject
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int DiplomProjectId { get; set; }

        public DateTime? ApproveDate { get; set; }

        public int? Mark { get; set; }

        public virtual DiplomProject DiplomProject { get; set; }
    }
}
