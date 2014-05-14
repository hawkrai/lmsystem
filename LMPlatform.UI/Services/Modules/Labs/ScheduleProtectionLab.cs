namespace LMPlatform.UI.Services.Modules.Labs
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ScheduleProtectionLab
    {
        [DataMember]
        public int ScheduleProtectionId { get; set; }

        [DataMember]
        public string Mark { get; set; }
    }
}