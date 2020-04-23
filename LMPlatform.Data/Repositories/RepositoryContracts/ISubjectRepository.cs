using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface ISubjectRepository : IRepositoryBase<Subject>
    {
        List<Subject> GetSubjects(int groupId = 0, int lecturerId = 0);
        
        List<Subject> GetSubjectsLite(int? groupId = null);
        
        SubjectNews SaveNews(SubjectNews news);

        void DeleteNews(SubjectNews news);

        void DeleteLection(Lectures lectures);

		bool IsSubjectName(string name, string id, int userId);

		bool IsSubjectShortName(string name, string id, int userId);

	    void DisableNews(int subjectId, bool disable);
    }
}