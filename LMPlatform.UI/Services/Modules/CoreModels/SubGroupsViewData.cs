namespace LMPlatform.UI.Services.Modules.CoreModels
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using LMPlatform.UI.Services.Modules.Labs;
    using LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel;

    [DataContract]
    public class SubGroupsViewData
    {
        [DataMember]
        public int SubGroupId { get; set; }

        [DataMember]
        public int GroupId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<StudentsViewData> Students { get; set; }

        [DataMember]
        public List<LabsViewData> Labs { get; set; }
            
        [DataMember]
        public List<ScheduleProtectionLabsViewData> ScheduleProtectionLabs { get; set; }
        
        [DataMember]
        public List<ScheduleProtectionLab> ScheduleProtectionLabsRecomendMark { get; set; }
    }
}