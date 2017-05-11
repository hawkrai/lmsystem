using System.Runtime.Serialization;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectMemberViewData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Role { get; set; }

        public ProjectMemberViewData(ProjectUser projectUser)
        {
            Id = projectUser.Id;
            UserId = projectUser.User.Id;
            Name = projectUser.User.FullName;
            Role = projectUser.ProjectRole.Name;
        }
    }
}