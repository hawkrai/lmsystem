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
    }
}