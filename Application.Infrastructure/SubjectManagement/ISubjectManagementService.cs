using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace Application.Infrastructure.SubjectManagement
{
    using System;

    using Application.Infrastructure.Models;

    public interface ISubjectManagementService
    {
        List<Subject> GetUserSubjects(int userId);

        List<Subject> GetGroupSubjects(int groupId);

        Subject GetSubject(int id);

        IPageableList<Subject> GetSubjectsLecturer(int lecturerId, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        Subject SaveSubject(Subject subject);

        SubjectNews SaveNews(SubjectNews news);

        void DeleteNews(SubjectNews news);

        void DeleteLection(Lectures lectures);

        void DeleteLectionVisitingDate(int id);

        void DeletePracticalsVisitingDate(int id);

        void DeleteLabsVisitingDate(int id);

        void DeleteLabs(int id);

        void DeletePracticals(int id);

        bool IsWorkingSubject(int userId, int subjectId);

        SubjectNews GetNews(int id, int subjecttId);

	    IList<SubGroup> GetSubGroups(int subjectId, int groupId);

		void SaveSubGroup(int subjectId, int groupId, IList<int> firstInts, IList<int> secoInts, IList<int> thirdInts);

        Lectures GetLectures(int id);

        Lectures SaveLectures(Lectures lectures, IList<Attachment> attachments, Int32 userId);

        Labs SaveLabs(Labs labs, IList<Attachment> attachments, Int32 userId);

		UserLabFiles SaveUserLabFiles(UserLabFiles userLabFiles, IList<Attachment> attachments);

        Labs GetLabs(int id);

        Practical GetPractical(int id);

        Practical SavePractical(Practical practical, IList<Attachment> attachments, Int32 userId);

        void SaveDateLectures(int subjectId, DateTime date);

        List<LecturesScheduleVisiting> GetScheduleVisitings(Query<LecturesScheduleVisiting> query);

        void SaveMarksCalendarData(List<LecturesVisitMark> lecturesVisitMarks);

        void SaveScheduleProtectionLabsDate(int subGroupId, DateTime date);

        void SaveScheduleProtectionPracticalDate(ScheduleProtectionPractical scheduleProtectionPractical);

        SubGroup GetSubGroup(int subGroupId);

        Group GetGroup(int groupId);

        void SaveLabsVisitingData(ScheduleProtectionLabMark protectionLabMarks);

        void SaveStudentLabsMark(StudentLabMark studentLabMark);

        void SavePracticalVisitingData(List<ScheduleProtectionPracticalMark> protectionPracticalMarks);

        void SavePracticalMarks(List<StudentPracticalMark> studentPracticalMarks);

        List<string> GetLecturesAttachments(int subjectId);

        List<string> GetLabsAttachments(int subjectId);

        List<string> GetPracticalsAttachments(int subjectId);

        void DeleteSubject(int id);

        List<Subject> GetSubjects();

	    List<UserLabFiles> GetUserLabFiles(int userId, int subjectId);

		UserLabFiles GetUserLabFile(int id);

		void DeleteUserLabFile(int id);

	    bool IsSubjectName(string name, string id);

	    bool IsSubjectShortName(string name, string id);

	    void DisableNews(int subjectId, bool disable);

        List<ProfileCalendarModel> GetLabEvents(int userId);

        List<ProfileCalendarModel> GetLecturesEvents(int userId);

        List<Subject> GetSubjectsByLector(int userId);

        List<Subject> GetSubjectsByStudent(int userId);

		decimal GetSubjectCompleting(int subjectId, string user, Student student);

        int StudentAttendance(int userId);

	    List<Subject> GetSubjectsByLectorOwner(int userId);

	    IList<SubGroup> GetSubGroupsV2(int subjectId, int groupId);

        IList<SubGroup> GetSubGroupsV3(int subjectId, int groupId);

        List<Labs> GetLabsV2(int subjectId);

	    IList<SubGroup> GetSubGroupsV2WithScheduleProtectionLabs(int subjectId, int groupId);

		Subject GetSubject(IQuery<Subject> query);

		List<SubjectNews> GetNewsByGroup(int id);

		List<SubjectNews> GetNewsByLector(int id);

		List<ProfileCalendarModel> GetGroupsLabEvents(int groupId, int userId);

		void UpdateUserLabFile(string userFileId, bool isReceived);

		UserLabFiles GetUserLabFile(string path);

		List<ProfileCalendarModel> GetLecturesEvents(int groupId, int userId);
	}
}