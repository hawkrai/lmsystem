namespace LMPlatform.UI.Services.Modules.Lectures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class StudentMarkForDateViewData
    {
        [DataMember]
        public int StudentId { get; set; }

        [DataMember]
        public string StudentName { get; set; }

        [DataMember]
        public string Mark { get; set; }

        [DataMember]
        public int MarkId { get; set; }
    }
}