using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface ISubjectRepository : IRepositoryBase<Subject>
    {
        List<Subject> GetSubjects(int groupId = 0, int lecturerId = 0);

        SubjectNews SaveNews(SubjectNews news);

        void DeleteNews(SubjectNews news);

        void DeleteLection(Lectures lectures);

		bool IsSubjectName(string name, string id);

		bool IsSubjectShortName(string name, string id);

	    void DisableNews(int subjectId, bool disable);
    }
}