using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace Application.Infrastructure.SubjectManagement
{
    using System;

    public interface ISubjectManagementService
    {
        List<Subject> GetUserSubjects(int userId);

        List<Subject> GetGroupSubjects(int groupId);

        Subject GetSubject(int id);

        IPageableList<Subject> GetSubjectsLecturer(int lecturerId, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        Subject SaveSubject(Subject subject);

        SubjectNews SaveNews(SubjectNews news);

        void DeleteNews(SubjectNews news);

        SubjectNews GetNews(int id, int subjecttId);

	    IList<SubGroup> GetSubGroups(int subjectId, int groupId);

        void SaveSubGroup(int subjectId, int groupId, IList<int> firstInts, IList<int> secoInts);

        Lectures GetLectures(int id);

        Lectures SaveLectures(Lectures lectures, IList<Attachment> attachments);

        Labs SaveLabs(Labs labs, IList<Attachment> attachments);

        Labs GetLabs(int id);

        Practical GetPractical(int id);

        Practical SavePractical(Practical practical, IList<Attachment> attachments);

        void SaveDateLectures(int subjectId, DateTime date);

        List<LecturesScheduleVisiting> GetScheduleVisitings(Query<LecturesScheduleVisiting> query);

        void SaveMarksCalendarData(List<LecturesVisitMark> lecturesVisitMarks);

        void SaveScheduleProtectionLabsDate(int subGroupId, DateTime date);

        void SaveScheduleProtectionPracticalDate(ScheduleProtectionPractical scheduleProtectionPractical);

        SubGroup GetSubGroup(int subGroupId);

        void SaveLabsVisitingData(List<ScheduleProtectionLabMark> protectionLabMarks);

        void SaveStudentLabsMark(List<StudentLabMark> studentLabMark);

        void SavePracticalVisitingData(List<ScheduleProtectionPracticalMark> protectionPracticalMarks);

        void SavePracticalMarks(List<StudentPracticalMark> studentPracticalMarks);

        List<string> GetLecturesAttachments(int subjectId);

        List<string> GetLabsAttachments(int subjectId);

        List<string> GetPracticalsAttachments(int subjectId);
    }
}