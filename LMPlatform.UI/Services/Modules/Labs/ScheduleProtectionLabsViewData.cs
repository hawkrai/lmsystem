namespace LMPlatform.UI.Services.Modules.Labs
{
    using System.Runtime.Serialization;

    using LMPlatform.Models;

    [DataContract]
    public class ScheduleProtectionLabsViewData
    {
        public ScheduleProtectionLabsViewData(ScheduleProtectionLabs scheduleProtectionLabs)
        {
            ScheduleProtectionLabId = scheduleProtectionLabs.Id;
            SubGroupId = scheduleProtectionLabs.SuGroupId;
            Date = scheduleProtectionLabs.Date.ToString("dd/MM/yyyy");
        }

        [DataMember]
        public int ScheduleProtectionLabId { get; set; }

        [DataMember]
        public int SubGroupId { get; set; }

        [DataMember]
        public string Date { get; set; }
    }
}