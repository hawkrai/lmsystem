using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.CoreModels
{
    [DataContract]
    public class StudentPracticalMarkViewData
    {
        [DataMember]
        public int PracticalId { get; set; }

        [DataMember]
        public int StudentId { get; set; }

        [DataMember]
        public string Mark { get; set; }

        [DataMember]
        public int StudentPracticalMarkId { get; set; }
    }
}