using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure.ConceptManagement
{
    public class ConceptManagementService : IConceptManagementService
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get
            {
                return filesManagementService.Value;
            }
        }

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
            }
        }

        public Concept GetTreeConceptByElementId(Int32 id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var concept = GetById(id);
                var elementId = 0;
                if (!concept.ParentId.HasValue)
                    elementId = concept.Id;
                else
                    FindRootId(concept, out elementId);
               return repositoriesContainer.ConceptRepository.GetTreeConceptByElementId(elementId);
            }
        }

        private void FindRootId(Concept concept, out Int32 elementId)
        {
            var c = GetById(concept.ParentId.GetValueOrDefault());
            if (!concept.ParentId.HasValue)
            {
                elementId = concept.Id;
                return;
            }
            else
                FindRootId(c, out elementId);
        }

        public Concept GetById(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ConceptRepository.GetById(id);
            }
        }

        public IEnumerable<Concept> GetRootElements(Int32 authorId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ConceptRepository.GetRootElementsByAuthorId(authorId);
            }
        }

        public IEnumerable<Concept> GetElementsByParentId(Int32 authorId, Int32 parentId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ConceptRepository.GetByParentId(parentId).Where(c => c.UserId == authorId);
            }
        }

        public Concept UpdateRootConcept(Int32 id, String name)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var concept = repositoriesContainer.ConceptRepository.GetById(id);
                concept.Name = name;
                repositoriesContainer.ConceptRepository.Save(concept);
                repositoriesContainer.ApplyChanges();

                return concept;
            }
        }

        public Concept SaveConcept(Concept concept)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                Concept source = null;
                if (concept.Id != 0)
                    source = GetById(concept.Id);

                repositoriesContainer.ConceptRepository.Save(concept);
                repositoriesContainer.ApplyChanges();
                BindNeighborConcept(concept, source, repositoriesContainer);
                return concept;
            }
        }

        private void BindNeighborConcept(Concept concept, Concept sourceConcept, LmPlatformRepositoriesContainer repositoriesContainer)
        {
            if (concept.PrevConcept.HasValue)
            {
                var leftConcept = repositoriesContainer.ConceptRepository.GetById(concept.PrevConcept.Value);
                leftConcept.NextConcept = concept.Id;
                repositoriesContainer.ConceptRepository.Save(leftConcept);
                repositoriesContainer.ApplyChanges();
            }
            if (!concept.PrevConcept.HasValue && sourceConcept!=null&& sourceConcept.PrevConcept.HasValue)
            {
                var leftConcept = repositoriesContainer.ConceptRepository.GetById(sourceConcept.PrevConcept.Value);
                leftConcept.NextConcept = null;
                repositoriesContainer.ConceptRepository.Save(leftConcept);
                repositoriesContainer.ApplyChanges();
            }

            if (concept.NextConcept.HasValue)
            {
                var rightConcept = repositoriesContainer.ConceptRepository.GetById(concept.NextConcept.Value);
                rightConcept.PrevConcept = concept.Id;
                repositoriesContainer.ConceptRepository.Save(rightConcept);
                repositoriesContainer.ApplyChanges();
            }
            if (!concept.NextConcept.HasValue && sourceConcept != null && sourceConcept.NextConcept.HasValue)
            {
                var rightConcept = repositoriesContainer.ConceptRepository.GetById(sourceConcept.NextConcept.Value);
                rightConcept.PrevConcept = null;
                repositoriesContainer.ConceptRepository.Save(rightConcept);
                repositoriesContainer.ApplyChanges();
            }
        }

        public Concept SaveConcept(Concept concept, IList<Attachment> attachments)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                if (!string.IsNullOrEmpty(concept.Container))
                {
                    var deleteFiles =
                        repositoriesContainer.AttachmentRepository.GetAll(
                            new Query<Attachment>(e => e.PathName == concept.Container)).ToList().Where(e => attachments.All(x => x.Id != e.Id)).ToList();

                    foreach (var attachment in deleteFiles)
                    {
                        FilesManagementService.DeleteFileAttachment(attachment);
                    }
                }
                else
                {
                    concept.Container = GetGuidFileName();
                }

                FilesManagementService.SaveFiles(attachments.Where(e => e.Id == 0), concept.Container);

                foreach (var attachment in attachments)
                {
                    if (attachment.Id == 0)
                    {
                        attachment.PathName = concept.Container;
                       
                        repositoriesContainer.AttachmentRepository.Save(attachment);
                    }
                }
                concept.Published = attachments.Any();
                Concept source = null;
                if (concept.Id != 0)
                    source = GetById(concept.Id);
                repositoriesContainer.ConceptRepository.Save(concept);
                repositoriesContainer.ApplyChanges();
                BindNeighborConcept(concept, source, repositoriesContainer);
                return concept;
            }
        }

        private void AttachFolderToSection(String folderName, Int32 userId, Int32 subjectId, String sectionName)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var parent = repositoriesContainer.ConceptRepository.GetBy(
                    new Query<Concept>()
                    .AddFilterClause(f => f.SubjectId == subjectId)
                    .AddFilterClause(f => f.UserId == userId)
                    .AddFilterClause(f => String.Compare(f.Name.Trim(), sectionName.Trim(), true) == 0)
                    .Include(c=>c.Author)
                    .Include(c=>c.Subject));

                var concept = new Concept(folderName, parent.Author, parent.Subject, true, false);
                concept.ParentId = parent.Id;
                concept.ReadOnly = true;
                repositoriesContainer.ConceptRepository.Save(concept);
            }
        }

        public void AttachFolderToLectSection(String folderName, Int32 userId, Int32 subjectId)
        {
            AttachFolderToSection(folderName, userId, subjectId, LectSectionName);
        }

        public void AttachFolderToLabSection(String folderName, Int32 userId, Int32 subjectId)
        {
            AttachFolderToSection(folderName, userId, subjectId, LabSectionName);
        }

        private string GetGuidFileName()
        {
            return string.Format("P{0}", Guid.NewGuid().ToString("N").ToUpper());
        }

        public Concept CreateRootConcept(String name, Int32 authorId, Int32 subjectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var author = repositoriesContainer.UsersRepository.GetBy(new Query<User>().AddFilterClause(u => u.Id == authorId));
                var subject = repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>().AddFilterClause(s => s.Id == subjectId));
                var concept = new Concept(name, author, subject, true, false);

                repositoriesContainer.ConceptRepository.Save(concept);
                repositoriesContainer.ApplyChanges();
                InitBaseChildrens(concept, repositoriesContainer);
                return repositoriesContainer.ConceptRepository.GetBy
                    (new Query<Concept>()
                    .AddFilterClause(c => c.Id == concept.Id));
            }
        }

        private const String LectSectionName = "Теоретический раздел";
        private const String LabSectionName = "Практический раздел";

        private void InitBaseChildrens(Concept parent, LmPlatformRepositoriesContainer repositoriesContainer)
        {
            var concept1 = new Concept("Титульный экран", parent.Author, parent.Subject, false, false);
            concept1.ParentId = parent.Id;
            concept1.ReadOnly = true;
            repositoriesContainer.ConceptRepository.Save(concept1);

            var concept2 = new Concept("Программа курса", parent.Author, parent.Subject, false, false);
            concept2.ParentId = parent.Id;
            concept2.ReadOnly = true;
            repositoriesContainer.ConceptRepository.Save(concept2);

            var concept3 = new Concept(LectSectionName, parent.Author, parent.Subject, true, false);
            concept3.ParentId = parent.Id;
            concept3.ReadOnly = true;
            repositoriesContainer.ConceptRepository.Save(concept3);
            InitLectChild(concept3, repositoriesContainer);

            var concept4 = new Concept(LabSectionName, parent.Author, parent.Subject, true, false);
            concept4.ParentId = parent.Id;
            concept4.ReadOnly = true;
            repositoriesContainer.ConceptRepository.Save(concept4);
            InitPractChild(concept4, repositoriesContainer);

            var concept5 = new Concept("Блок контроля знаний", parent.Author, parent.Subject, true, false);
            concept5.ParentId = parent.Id;
            concept5.ReadOnly = true;
            repositoriesContainer.ConceptRepository.Save(concept5);
            repositoriesContainer.ApplyChanges();
        }

        private void InitLectChild(Concept parent, LmPlatformRepositoriesContainer repositoriesContainer)
        {
            var sub = SubjectManagementService.GetSubject(parent.SubjectId);
            foreach (var item in sub.Lectures)
            {
                var concept = new Concept(item.Theme, parent.Author, parent.Subject, true, false);
                concept.ParentId = parent.Id;
                concept.ReadOnly = true;
                repositoriesContainer.ConceptRepository.Save(concept);
            }
        }

        private void InitPractChild(Concept parent, LmPlatformRepositoriesContainer repositoriesContainer)
        {
            var sub = SubjectManagementService.GetSubject(parent.SubjectId);
            foreach (var item in sub.Practicals)
            {
                var concept = new Concept(item.Theme, parent.Author, parent.Subject, true, false);
                concept.ParentId = parent.Id;
                concept.ReadOnly = true;
                repositoriesContainer.ConceptRepository.Save(concept);
            }
        }

        public void Remove(Int32 id, Boolean removeChildren)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.ConceptRepository.Remove(id, removeChildren);
                repositoriesContainer.ApplyChanges();
            }
        }
    }
}
