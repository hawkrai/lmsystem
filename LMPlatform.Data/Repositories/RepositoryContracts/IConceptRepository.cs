using Application.Core.Data;
using LMPlatform.Models;
using System.Collections.Generic;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IConceptRepository : IRepositoryBase<Concept>
    {
        Concept GetById(int id);
        Concept GetTreeConceptByElementId(int elementId);
        IEnumerable<Concept> GetRootElementsByAuthorId(int authorId);
        IEnumerable<Concept> GetRootElementsBySubjectId(int subjectId);
        IEnumerable<Concept> GetByParentId(int id);
        IEnumerable<Concept> GetBySubjectId(int subjectId);
        IEnumerable<Concept> GetByAuthorId(int authorId);
        void Remove(int id, bool removeChildren);
    }
}
