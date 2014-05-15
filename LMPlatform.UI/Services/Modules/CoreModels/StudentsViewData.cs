namespace LMPlatform.UI.Services.Modules.CoreModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using LMPlatform.Models;
    using LMPlatform.UI.Services.Modules.Labs;

    [DataContract]
    public class StudentsViewData
    {
        public StudentsViewData(Student student, IEnumerable<ScheduleProtectionLabs> scheduleProtectionLabs = null, IEnumerable<Labs> labs = null)
        {
            StudentId = student.Id;
            FullName = student.FullName;
            GroupId = student.GroupId;
            LabVisitingMark = new List<LabVisitingMarkViewData>();
            StudentLabMarks = new List<StudentLabMarkViewData>();

            if (labs != null)
            {
                foreach (var lab in labs)
                {
                    if (student.StudentLabMarks.Any(e => e.LabId == lab.Id))
                    {
                        StudentLabMarks.Add(new StudentLabMarkViewData
                                                {
                                                    LabId = lab.Id,
                                                    Mark = student.StudentLabMarks.FirstOrDefault(e => e.LabId == lab.Id).Mark,
                                                    StudentId = StudentId,
                                                    StudentLabMarkId = student.StudentLabMarks.FirstOrDefault(e => e.LabId == lab.Id).Id
                                                });        
                    }
                    else
                    {
                        StudentLabMarks.Add(new StudentLabMarkViewData
                        {
                            LabId = lab.Id,
                            Mark = string.Empty,
                            StudentId = StudentId,
                            StudentLabMarkId = 0
                        });    
                    }
                }   
            }

            if (scheduleProtectionLabs != null)
            {
                foreach (var scheduleProtectionLab in scheduleProtectionLabs)
                {
                    if (student.ScheduleProtectionLabMarks.Any(e => e.ScheduleProtectionLabId == scheduleProtectionLab.Id))
                    {
                        this.LabVisitingMark.Add(new LabVisitingMarkViewData
                                                {
                                                    Comment = student.ScheduleProtectionLabMarks.FirstOrDefault(e => e.ScheduleProtectionLabId == scheduleProtectionLab.Id).Comment,
                                                    Mark = student.ScheduleProtectionLabMarks.FirstOrDefault(e => e.ScheduleProtectionLabId == scheduleProtectionLab.Id).Mark,
                                                    ScheduleProtectionLabId = scheduleProtectionLab.Id,
                                                    StudentId = this.StudentId,
                                                    LabVisitingMarkId = student.ScheduleProtectionLabMarks.FirstOrDefault(e => e.ScheduleProtectionLabId == scheduleProtectionLab.Id).Id
                                                });    
                    }
                    else
                    {
                        this.LabVisitingMark.Add(new LabVisitingMarkViewData
                                                {
                                                    Comment = string.Empty,
                                                    Mark = string.Empty,
                                                    ScheduleProtectionLabId = scheduleProtectionLab.Id,
                                                    StudentId = this.StudentId,
                                                    LabVisitingMarkId = 0
                                                });       
                    }
                }
            }
        }

        [DataMember]
        public int StudentId { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public int GroupId { get; set; }

        [DataMember]
        public List<LabVisitingMarkViewData> LabVisitingMark { get; set; }

        [DataMember]
        public List<StudentLabMarkViewData> StudentLabMarks { get; set; }
    }
}