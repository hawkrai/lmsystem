using System.Runtime.Serialization;
using LMPlatform.Models.BTS;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectMatrixRequirementViewData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool Covered { get; set; }

        public ProjectMatrixRequirementViewData(ProjectMatrixRequirement requirement)
        {
            Id = requirement.Id;
            Number = requirement.Number;
            Name = requirement.Name;
            Covered = requirement.Covered;
        }
    }
}