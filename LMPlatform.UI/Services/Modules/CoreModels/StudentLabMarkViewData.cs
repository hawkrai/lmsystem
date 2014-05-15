namespace LMPlatform.UI.Services.Modules.CoreModels
{
    using System.Runtime.Serialization;

    [DataContract]
    public class StudentLabMarkViewData
    {
        [DataMember]
        public int LabId { get; set; }

        [DataMember]
        public int StudentId { get; set; }

        [DataMember]
        public string Mark { get; set; }

        [DataMember]
        public int StudentLabMarkId { get; set; }
    }
}