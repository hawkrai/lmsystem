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
        public List<StudentsViewData> Students { get; set; }

        [DataMember]
        public List<StudentsViewData> SubGroupsOne { get; set; }

        [DataMember]
        public List<StudentsViewData> SubGroupsTwo { get; set; }
    }
}