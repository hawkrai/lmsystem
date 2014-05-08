namespace LMPlatform.UI.Services.Modules.CoreModels
{
    using System.Runtime.Serialization;

    using LMPlatform.Models;

    [DataContract]
    public class StudentsViewData
    {
        public StudentsViewData(Student student)
        {
            StudentId = student.Id;
            FullName = student.FullName;
            GroupId = student.GroupId;
        }

        [DataMember]
        public int StudentId { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public int GroupId { get; set; }
    }
}