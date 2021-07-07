using LMPlatform.Models;
using System.Collections.Generic;

namespace Application.Infrastructure.ConceptManagement
{
    public interface IConceptManagementService
    {
	    Concept GetLiteById(int id);
        Concept GetById(int id);
        Concept GetByIdFixed(int id, bool withPrev = true, bool withNext = true);
        Concept GetTreeConceptByElementId(int elementId);
        IEnumerable<Concept> GetRootElements(int authorId, bool onlyVisible=false);
        IEnumerable<Concept> GetRootElementsBySubject(int subjectId);
        IEnumerable<Concept> GetRootTreeElementsBySubject(int subjectId);
        IEnumerable<Concept> GetElementsByParentId(int parentId, int authorId);
        IEnumerable<Concept> GetElementsByParentId(int parentId);
        IEnumerable<Concept> GetElementsBySubjectId(int subjectId);
        Concept CreateRootConcept(string name, int authorId, int subjectId);
        Concept SaveConcept(Concept concept, IList<Attachment> attachments);
        Concept SaveConcept(Concept concept);
        Concept UpdateRootConcept(int id, string name, bool published);
        void Remove(int id, bool removeChildren);
        Concept AttachSiblings(int sourceId, int rightId, int leftId);
        void AttachFolderToLectSection(string folderName, int userId, int subjectId);
        void AttachFolderToLabSection(string folderName, int userId, int subjectId);
        bool IsTestModule(string moduleName);
    }
}
