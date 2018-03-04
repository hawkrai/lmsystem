using LMPlatform.UI.Services.Modules.Lectures;
using LMPlatform.UI.Services.Modules.Practicals;

namespace LMPlatform.UI.Services.Modules.CoreModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using LMPlatform.Models;

    [DataContract]
    public class GroupsViewData
    {
        [DataMember]
        public int GroupId { get; set; }

        [DataMember]
        public string GroupName { get; set; }

		[DataMember]
		public int CountUnconfirmedStudents { get; set; }

        [DataMember]
        public List<StudentsViewData> Students { get; set; }

        [DataMember]
        public SubGroupsViewData SubGroupsOne { get; set; }

        [DataMember]
        public SubGroupsViewData SubGroupsTwo { get; set; }

		[DataMember]
		public SubGroupsViewData SubGroupsThird { get; set; }

        [DataMember]
        public List<LecturesMarkVisitingViewData> LecturesMarkVisiting { get; set; }

        [DataMember]
        public List<ScheduleProtectionPracticalViewData> ScheduleProtectionPracticals { get; set; }
    }
}