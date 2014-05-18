using System.Linq;
using System.Runtime.Serialization;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.Practicals
{
    [DataContract]
    public class PracticalVisitingMarkViewData
    {
        public PracticalVisitingMarkViewData()
        {
        }

        public PracticalVisitingMarkViewData(Student student, int scheduleId)
        {
            ScheduleProtectionPracticalId = scheduleId;
            StudentId = student.Id;
            StudentName = student.FullName;
            if (student.ScheduleProtectionPracticalMarks.Any(e => e.ScheduleProtectionPracticalId == scheduleId))
            {
                Comment =
                    student.ScheduleProtectionPracticalMarks.FirstOrDefault(e => e.ScheduleProtectionPracticalId == scheduleId)
                        .Comment;
                Mark =
                    student.ScheduleProtectionPracticalMarks.FirstOrDefault(e => e.ScheduleProtectionPracticalId == scheduleId).Mark;
                PracticalVisitingMarkId = student.ScheduleProtectionPracticalMarks.FirstOrDefault(e => e.ScheduleProtectionPracticalId == scheduleId).Id;
            }
            else
            {
                Comment = string.Empty;
                Mark = string.Empty;
                PracticalVisitingMarkId = 0;
            }
        }

        [DataMember]
        public int ScheduleProtectionPracticalId { get; set; }

        [DataMember]
        public int StudentId { get; set; }

        [DataMember]
        public string StudentName { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string Mark { get; set; }

        [DataMember]
        public int PracticalVisitingMarkId { get; set; } 
    }
}