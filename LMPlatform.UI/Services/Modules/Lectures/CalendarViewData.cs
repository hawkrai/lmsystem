namespace LMPlatform.UI.Services.Modules.Lectures
{
    using System.Runtime.Serialization;

    using LMPlatform.Models;

    [DataContract]
    public class CalendarViewData
    {
        public CalendarViewData(LecturesScheduleVisiting visiting)
        {
            SubjectId = visiting.SubjectId;
            Date = visiting.Date.ToString("d/M/yyy");
            Id = visiting.Id;
        }

        [DataMember]
        public int SubjectId { get; set; }

        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public int Id { get; set; }
    }
}