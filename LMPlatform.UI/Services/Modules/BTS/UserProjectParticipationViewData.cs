using System.Linq;
using System.Runtime.Serialization;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class UserProjectParticipationViewData
    {
        [DataMember]
        public int ProjectId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string CreatorName { get; set; }

        [DataMember]
        public string UserRole { get; set; }

        public UserProjectParticipationViewData(Project project, int userId)
        {
            ProjectId = project.Id;
            Title = project.Title;
            CreatorName = project.Creator.FullName;
            UserRole = project.ProjectUsers.First(e => e.UserId == userId).ProjectRole.Name;
        }
    }
}