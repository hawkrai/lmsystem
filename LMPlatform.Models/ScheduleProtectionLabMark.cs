using Application.Core.Data;

namespace LMPlatform.Models
{
    public class ScheduleProtectionLabMark : ModelBase
    {
        public int ScheduleProtectionLabId { get; set; }

        public int StudentId { get; set; }

        public string Comment { get; set; }

        public string Mark { get; set; }

        public Student Student { get; set; }
        
        public ScheduleProtectionLabs ScheduleProtectionLab { get; set; }

        public ScheduleProtectionLabMark()
        {

        }

        public ScheduleProtectionLabMark(int id, int studentId, string comment, string mark, int scheduleProtectionId)
        {
            Id = id;
            StudentId = studentId;
            Comment = comment;
            Mark = mark;
            ScheduleProtectionLabId = scheduleProtectionId;
        }
    }
}