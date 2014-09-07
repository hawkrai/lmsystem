using LMPlatform.UI.Services.Modules.Practicals;

namespace LMPlatform.UI.Services.Modules.CoreModels
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;

    using LMPlatform.Models;
    using LMPlatform.UI.Services.Modules.Labs;

    [DataContract]
    public class StudentsViewData
    {
        public StudentsViewData()
        {
        }

        public StudentsViewData(Student student, IEnumerable<ScheduleProtectionLabs> scheduleProtectionLabs = null, IEnumerable<ScheduleProtectionPractical> scheduleProtectionPracticals = null, IEnumerable<Labs> labs = null, IEnumerable<Practical> practicals = null, List<UserlabFilesViewData> userLabsFile = null)
        {
            StudentId = student.Id;
            FullName = student.FullName;
            GroupId = student.GroupId;
            LabVisitingMark = new List<LabVisitingMarkViewData>();
            PracticalVisitingMark = new List<PracticalVisitingMarkViewData>();
            StudentLabMarks = new List<StudentLabMarkViewData>();
            StudentPracticalMarks = new List<StudentPracticalMarkViewData>();

            FileLabs = userLabsFile;

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

            if (practicals != null)
            {
                foreach (var practical in practicals)
                {
                    if (student.StudentPracticalMarks.Any(e => e.PracticalId == practical.Id))
                    {
                        StudentPracticalMarks.Add(new StudentPracticalMarkViewData
                        {
                            PracticalId = practical.Id,
                            Mark = student.StudentPracticalMarks.FirstOrDefault(e => e.PracticalId == practical.Id).Mark,
                            StudentId = StudentId,
                            StudentPracticalMarkId = student.StudentPracticalMarks.FirstOrDefault(e => e.PracticalId == practical.Id).Id
                        });
                    }
                    else
                    {
                        StudentPracticalMarks.Add(new StudentPracticalMarkViewData
                        {
                            PracticalId = practical.Id,
                            Mark = string.Empty,
                            StudentId = StudentId,
                            StudentPracticalMarkId = 0
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

            if (scheduleProtectionPracticals != null)
            {
                foreach (var scheduleProtectionPractical in scheduleProtectionPracticals)
                {
                    if (student.ScheduleProtectionPracticalMarks.Any(e => e.ScheduleProtectionPracticalId == scheduleProtectionPractical.Id))
                    {
                        this.PracticalVisitingMark.Add(new PracticalVisitingMarkViewData
                        {
                            Comment = student.ScheduleProtectionPracticalMarks.FirstOrDefault(e => e.ScheduleProtectionPracticalId == scheduleProtectionPractical.Id).Comment,
                            Mark = student.ScheduleProtectionPracticalMarks.FirstOrDefault(e => e.ScheduleProtectionPracticalId == scheduleProtectionPractical.Id).Mark,
                            ScheduleProtectionPracticalId = scheduleProtectionPractical.Id,
                            StudentId = this.StudentId,
                            PracticalVisitingMarkId = student.ScheduleProtectionPracticalMarks.FirstOrDefault(e => e.ScheduleProtectionPracticalId == scheduleProtectionPractical.Id).Id
                        });
                    }
                    else
                    {
                        this.PracticalVisitingMark.Add(new PracticalVisitingMarkViewData
                        {
                            Comment = string.Empty,
                            Mark = string.Empty,
                            ScheduleProtectionPracticalId = scheduleProtectionPractical.Id,
                            StudentId = this.StudentId,
                            PracticalVisitingMarkId = 0
                        });
                    }
                }
            }

            var summ = this.StudentLabMarks.Where(studentLabMarkViewData => !string.IsNullOrEmpty(studentLabMarkViewData.Mark)).Sum(studentLabMarkViewData => double.Parse(studentLabMarkViewData.Mark));
            if (StudentLabMarks.Count() != 0)
            {
                LabsMarkTotal = (summ / StudentLabMarks.Count()).ToString(CultureInfo.InvariantCulture);    
            }

            summ = this.StudentPracticalMarks.Where(studentPracticalMarkViewData => !string.IsNullOrEmpty(studentPracticalMarkViewData.Mark)).Sum(studentPracticalMarkViewData => double.Parse(studentPracticalMarkViewData.Mark));

            var countMark =
                this.StudentPracticalMarks.Count(studentPracticalMarkViewData => !string.IsNullOrEmpty(studentPracticalMarkViewData.Mark));
            if (countMark != 0)
            {
                PracticalMarkTotal = (summ / countMark).ToString(CultureInfo.InvariantCulture);
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
        public List<PracticalVisitingMarkViewData> PracticalVisitingMark { get; set; }

        [DataMember]
        public List<StudentLabMarkViewData> StudentLabMarks { get; set; }

        [DataMember]
        public string LabsMarkTotal { get; set; }

        [DataMember]
        public string PracticalMarkTotal { get; set; }

        [DataMember]
        public List<StudentPracticalMarkViewData> StudentPracticalMarks { get; set; }

        [DataMember]
        public List<UserlabFilesViewData> FileLabs { get; set; } 
    }
}