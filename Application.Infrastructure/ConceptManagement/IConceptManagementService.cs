using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure.ConceptManagement
{
    public interface IConceptManagementService
    {
        Concept GetById(Int32 id);
        Concept GetTreeConceptByElementId(Int32 elementId);
        IEnumerable<Concept> GetRootElements(Int32 authorId);
        IEnumerable<Concept> GetRootElementsBySubject(Int32 subjectId);
        IEnumerable<Concept> GetElementsByParentId(Int32 authorId, Int32 parentId);
        IEnumerable<Concept> GetElementsByParentId(Int32 parentId);
        Concept CreateRootConcept(String name, Int32 authorId, Int32 subjectId);
        Concept SaveConcept(Concept concept, IList<Attachment> attachments);
        Concept SaveConcept(Concept concept);
        Concept UpdateRootConcept(Int32 id, String name);
        void Remove(Int32 id, Boolean removeChildren);
        void AttachFolderToLectSection(String folderName, Int32 userId, Int32 subjectId);
        void AttachFolderToLabSection(String folderName, Int32 userId, Int32 subjectId);
    }
}
