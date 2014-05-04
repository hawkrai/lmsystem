using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMPlatform.Models.DP
{
    public class DiplomProject
    {
        public DiplomProject()
        {
            AssignedDiplomProjects = new HashSet<AssignedDiplomProject>();
            DiplomProjectGroups = new HashSet<DiplomProjectGroup>();
        }

        public int DiplomProjectId { get; set; }

        [Required]
        [StringLength(2048)]
        public string Theme { get; set; }

        public int LecturerId { get; set; }

        public string InputData { get; set; }

        public string RpzContent { get; set; }

        public string DrawMaterials { get; set; }

        public string Consultants { get; set; }

        public string Workflow { get; set; }

        public virtual ICollection<AssignedDiplomProject> AssignedDiplomProjects { get; set; }

        public virtual ICollection<DiplomProjectGroup> DiplomProjectGroups { get; set; }

        public virtual Lecturer Lecturer { get; set; }

//        public virtual ICollection<Group> Groups { get; set; }
    }
}
