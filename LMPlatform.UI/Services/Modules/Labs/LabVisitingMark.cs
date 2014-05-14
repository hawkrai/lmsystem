namespace LMPlatform.UI.Services.Modules.Labs
{
    using System.Runtime.Serialization;

    [DataContract]
    public class LabVisitingMarkViewData
    {
        [DataMember]
        public int ScheduleProtectionLabId { get; set; }

        [DataMember]
        public int StudentId { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string Mark { get; set; }

        [DataMember]
        public int LabVisitingMarkId { get; set; }
    }
}