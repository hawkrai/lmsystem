namespace LMPlatform.UI.Services.Modules.Lectures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class StudentMarkForDateResult : ResultViewData
    {
        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public int DateId { get; set; }

        [DataMember]
        public List<StudentMarkForDateViewData> StudentMarkForDate { get; set; } 
    }
}