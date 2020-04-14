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
        public Dictionary<int, int> userLecturePass;

        [DataMember]
        public Dictionary<int, int> userLabPass;

        [DataMember]
        public Dictionary<int, double> userAvgLabMarks;

        [DataMember]
        public Dictionary<int, double> userAvgTestMarks;

        [DataMember]
        public Dictionary<int, int> userLabCount;

        [DataMember]
        public Dictionary<int, int> userTestCount;

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
            this.userLecturePass = new Dictionary<int, int>();
            this.userLabPass = new Dictionary<int, int>();
            this.userAvgLabMarks = new Dictionary<int, double>();
            this.userAvgTestMarks = new Dictionary<int, double>();
            this.userLabCount = new Dictionary<int, int>();
            this.userTestCount = new Dictionary<int, int>();
            this.subjects = subjects;

            foreach (var subject in subjects)
            {
                this.userLecturePass.Add(subject.Id, 0);
                this.userLabPass.Add(subject.Id, 0);
                this.userAvgLabMarks.Add(subject.Id, 0);
                this.userLabCount.Add(subject.Id, 0);
                this.userAvgTestMarks.Add(subject.Id, 0);
                this.userTestCount.Add(subject.Id, 0);
            }
            initLecturePass(student);
            initLabPass(student);
            initAvgLabMarks(student);
            initAvgTestMarks(student);
            foreach (var subject in subjects)
            {
                if (this.userLabCount[subject.Id] != 0)
                    this.userAvgLabMarks[subject.Id] /= this.userLabCount[subject.Id];
                if (this.userTestCount[subject.Id] != 0)
                    this.userAvgTestMarks[subject.Id] /= this.userTestCount[subject.Id];
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
                            this.userLecturePass[lecture.LecturesScheduleVisiting.SubjectId] += mark;
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
                        this.userLabPass[lab.ScheduleProtectionLab.SubGroup.SubjectGroup.SubjectId] += pass;
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
                    this.userAvgLabMarks[lab.Lab.SubjectId] += mark;
                    this.userLabCount[lab.Lab.SubjectId]++;

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
                            this.userAvgTestMarks[sub.Id] += (double)test.Points;
                            this.userTestCount[sub.Id]++;
                        }
                    }
                }
            }
        }
    }


}
