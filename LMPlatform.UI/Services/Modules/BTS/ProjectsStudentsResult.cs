using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class StudentsParticipationsResult : ResultViewData
    {
        [DataMember]
        public List<StudentParticipationViewData> ProjectsStudents { get; set; }

        [DataMember]
        public int TotalCount { get; set; }
    }
}