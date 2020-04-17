using Application.Core;
using Application.Infrastructure.KnowledgeTestsManagement;
using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace LMPlatform.UI.Services.Parental.Models
{
    [DataContract]
    public class ParentalUser
    {

        [DataMember]
        public string FIO;

        [DataMember]
        public int Id;

        [DataMember]
        public Dictionary<int, int> UserLecturePass;

        [DataMember]
        public Dictionary<int, int> UserLabPass;

        [DataMember]
        public Dictionary<int, double> UserAvgLabMarks;

        [DataMember]
        public Dictionary<int, double> UserAvgTestMarks;

        [DataMember]
        public Dictionary<int, int> UserLabCount;

        [DataMember]
        public Dictionary<int, int> UserTestCount;

        [DataMember]
        public double Rating;

        private List<Subject> subjects;

        private readonly LazyDependency<ITestPassingService> _testPassingService = new LazyDependency<ITestPassingService>();

        public ITestPassingService TestPassingService
        {
            get
            {
                return _testPassingService.Value;
            }
        }


        public ParentalUser(Student student, List<Subject> subjects)
        {
            #region Init
            FIO = student.FullName;
            Id = student.Id;
            this.UserLecturePass = new Dictionary<int, int>();
            this.UserLabPass = new Dictionary<int, int>();
            this.UserAvgLabMarks = new Dictionary<int, double>();
            this.UserAvgTestMarks = new Dictionary<int, double>();
            this.UserLabCount = new Dictionary<int, int>();
            this.UserTestCount = new Dictionary<int, int>();
            this.subjects = subjects;

            foreach (var subject in subjects)
            {
                this.UserLecturePass.Add(subject.Id, 0);
                this.UserLabPass.Add(subject.Id, 0);
                this.UserAvgLabMarks.Add(subject.Id, 0);
                this.UserLabCount.Add(subject.Id, 0);
                this.UserAvgTestMarks.Add(subject.Id, 0);
                this.UserTestCount.Add(subject.Id, 0);
            }
            initLecturePass(student);
            initLabPass(student);
            initAvgLabMarks(student);
            initAvgTestMarks(student);
            foreach (var subject in subjects)
            {
                if (this.UserLabCount[subject.Id] != 0)
                    this.UserAvgLabMarks[subject.Id] /= this.UserLabCount[subject.Id];
                if (this.UserTestCount[subject.Id] != 0)
                    this.UserAvgTestMarks[subject.Id] /= this.UserTestCount[subject.Id];
            }
            #endregion
        }

        private void initLecturePass(Student student)
        {
            if (student.LecturesVisitMarks != null) ;
            {
                foreach (var lecture in student.LecturesVisitMarks)
                {
                    if (lecture != null)
                    {
                        if (lecture.LecturesScheduleVisiting != null && lecture.Mark != null)
                        {
                            int mark;
                            int.TryParse(lecture.Mark, out mark);
                            this.UserLecturePass[lecture.LecturesScheduleVisiting.SubjectId] += mark;
                        }
                    }
                }
            }
        }

        private void initLabPass(Student student)
        {
            if (student.ScheduleProtectionLabMarks != null)
            {
                foreach (var lab in student.ScheduleProtectionLabMarks)
                {

                    if (lab != null && lab.ScheduleProtectionLab != null && lab.ScheduleProtectionLab.SubGroup != null && lab.ScheduleProtectionLab.SubGroup.SubjectGroup != null)
                    {
                        int pass;
                        Int32.TryParse(lab.Mark, out pass);
                        this.UserLabPass[lab.ScheduleProtectionLab.SubGroup.SubjectGroup.SubjectId] += pass;
                    }
                }
            }
        }

        private void initAvgLabMarks(Student student)
        {
            if (student.StudentLabMarks != null)
            {
                foreach (var lab in student.StudentLabMarks)
                {
                    double mark;
                    double.TryParse(lab.Mark, out mark);
                    this.UserAvgLabMarks[lab.Lab.SubjectId] += mark;
                    this.UserLabCount[lab.Lab.SubjectId]++;
                }
            }
        }

        private void initAvgTestMarks(Student student)
        {

            foreach (var sub in subjects)
            {
                var tests = TestPassingService.GetStidentResults(sub.Id, student.Id);
                if (tests != null)
                {
                    foreach (var test in tests)
                    {
                        if (test.Points != null)
                        {
                            this.UserAvgTestMarks[sub.Id] += (double)test.Points;
                            this.UserTestCount[sub.Id]++;
                        }
                    }
                }
            }
        }
    }


}
