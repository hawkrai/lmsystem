using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.Lectures
{
    using Models;

    [DataContract]
    public class CalendarViewData
    {
        public CalendarViewData(LecturesScheduleVisiting visiting)
        {
            SubjectId = visiting.SubjectId;
            Date = visiting.Date.ToString("dd/MM/yyy");
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
