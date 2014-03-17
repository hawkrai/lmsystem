using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace Application.Infrastructure.SubjectManagement
{
    public interface ISubjectManagementService
    {
        List<Subject> GetUserSubjects(int userId);

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
    }
}