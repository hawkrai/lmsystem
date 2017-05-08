using System.Collections.Generic;
using System.Runtime.Serialization;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class StudentParticipationViewData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public List<UserProjectParticipationViewData> Participations { get; set; }

        public StudentParticipationViewData(Student student)
        {
            Id = student.Id;
            FullName = student.FullName;
            Participations = new List<UserProjectParticipationViewData>();
            foreach(var projectUser in student.User.ProjectUsers)
            {
                Participations.Add(new UserProjectParticipationViewData(projectUser.Project, student.Id));
            }
        }
    }
}