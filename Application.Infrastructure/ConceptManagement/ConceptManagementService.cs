using Application.Core;
using Application.Core.Data;
using Application.Core.PdfConvertor;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Configuration;
using Application.Infrastructure.KnowledgeTestsManagement;

namespace Application.Infrastructure.ConceptManagement
{
    public class ConceptManagementService : IConceptManagementService
    {
        private const string TitlePageSectionName = "Титульный экран";
        private const string ProgramSectionName = "Программа курса";
        private const string LectSectionName = "Теоретический раздел";
        private const string LabSectionName = "Практический раздел";
        private const string TestSectionName = "Блок контроля знаний";


        private readonly string _storageRootTemp = ConfigurationManager.AppSettings["FileUploadPathTemp"];
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();
        private readonly LazyDependency<IModulesManagementService> _modulesManagementService = new LazyDependency<IModulesManagementService>();
        private readonly LazyDependency<ITestsManagementService> _testManagementService = new LazyDependency<ITestsManagementService>();

        public IFilesManagementService FilesManagementService => filesManagementService.Value;

        public ISubjectManagementService SubjectManagementService => subjectManagementService.Value;

        public IModulesManagementService ModulesManagementService => _modulesManagementService.Value;

        public ITestsManagementService TestsManagementService => _testManagementService.Value;

        public Concept AttachSiblings(int sourceId, int rightId, int leftId)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        Func<int, Query<Concept>> queryById = id => new Query<Concept>(c => c.Id == id);
	        var concept = repositoriesContainer.ConceptRepository.GetBy(queryById(sourceId));
	        var right = repositoriesContainer.ConceptRepository.GetBy(queryById(rightId));
            var left = repositoriesContainer.ConceptRepository.GetBy(queryById(leftId));
            var currentPrevId = concept.PrevConcept.GetValueOrDefault();
	        var currentNextId = concept.NextConcept.GetValueOrDefault();
	        concept.NextConcept = rightId > 0 ? rightId : (int?)null;
	        concept.PrevConcept = leftId > 0 ? leftId : (int?)null;
	        repositoriesContainer.ConceptRepository.Save(concept);
	        if (right != null)
	        {
		        right.PrevConcept = concept.Id;
		        repositoriesContainer.ConceptRepository.Save(right);
	        }     
	        if (left != null)
	        {
		        left.NextConcept = concept.Id;
		        repositoriesContainer.ConceptRepository.Save(left);
	        }
	        var currentPrev = repositoriesContainer.ConceptRepository.GetBy(queryById(currentPrevId));
	        var currentNext = repositoriesContainer.ConceptRepository.GetBy(queryById(currentNextId));

	        if(currentPrev!=null)
	        {
		        currentPrev.NextConcept = currentNext?.Id;
		        repositoriesContainer.ConceptRepository.Save(currentPrev);
	        }
	        if (currentNext != null)
	        {
		        currentNext.PrevConcept = currentPrev?.Id;
		        repositoriesContainer.ConceptRepository.Save(currentNext);
	        }
	        repositoriesContainer.ApplyChanges();

	        return concept;
        }

        public Concept GetTreeConceptByElementId(int id)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        var concept = GetLiteById(id);
	        int elementId;
	        if (!concept.ParentId.HasValue)
	        {
		        elementId = concept.Id;
	        }
	        else
	        {
		        FindRootId(concept, out elementId);
	        }

	        var res = repositoriesContainer.ConceptRepository.GetTreeConceptByElementId(elementId);
                
	        return AttachTestModuleData(res);
	        //return res;
        }

        private Concept AttachTestModuleData(Concept root)
        {
            var testData = TestsManagementService.GetTestsForSubject(root.SubjectId).Where(x => x.ForSelfStudy);
            var testModule = root.Children.FirstOrDefault(c => string.CompareOrdinal(c.Name, TestSectionName) == 0);
            if (testModule != null)
            {
                var tests = testData.Select(t => new Concept(t.Title, root.Author, root.Subject, false, t.Unlocked)
                {
	                Id = t.Id, Container = "test", ParentId = testModule.Id
                }).ToList();
                tests.ForEach(t => testModule.Children.Add(t));
            }

            return root;
        }

        private void FindRootId(Concept concept, out int elementId)
        {
            var c = GetLiteById(concept.ParentId.GetValueOrDefault());
            if (!concept.ParentId.HasValue)
            {
                elementId = concept.Id;
                return;
            }

            FindRootId(c, out elementId);
        }

        public Concept GetById(int id)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        var parent = repositoriesContainer.ConceptRepository.GetById(id);
	        return parent;
        }

        public Concept GetLiteById(int id)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
            var query = new Query<Concept>(c => c.Id == id);
	        var parent = repositoriesContainer.ConceptRepository.GetBy(query);
	        return parent;
        }

        public IEnumerable<Concept> GetRootElements(int authorId, bool onlyVisible=false)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        var rootElements = repositoriesContainer.ConceptRepository.GetRootElementsByAuthorId(authorId);
	        return !onlyVisible
		        ? rootElements
		        : rootElements.Where(re =>
			        ModulesManagementService.GetModules(re.SubjectId)
				        .Any(m => m.ModuleType == ModuleType.ComplexMaterial));
        }

        public IEnumerable<Concept> GetRootElementsBySubject(int subjectId)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        return repositoriesContainer.ConceptRepository.GetRootElementsBySubjectId(subjectId);
        }

        public IEnumerable<Concept> GetRootTreeElementsBySubject(int subjectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var roots = repositoriesContainer.ConceptRepository.GetRootElementsBySubjectId(subjectId);
                var res = new List<Concept>();
                foreach (var item in roots)
                    res.Add(GetTreeConceptByElementId(item.Id));
                return res;
            }
        }

        public IEnumerable<Concept> GetElementsBySubjectId(int subjectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ConceptRepository.GetBySubjectId(subjectId);
            }
        }

        public IEnumerable<Concept> GetElementsByParentId(int parentId)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        var parent = GetById(parentId);
	        if (IsTestModule(parent.Name))
	        {
		        return TestsManagementService.GetTestsForSubject(parent.SubjectId).Where(x => x.ForSelfStudy).Select(t => new Concept(t.Title, parent.Author, parent.Subject, false, t.Unlocked) { Id = t.Id, Container="test"});
	        }

	        return repositoriesContainer.ConceptRepository.GetByParentId(parentId);
        }

        public Concept GetByIdFixed(int id, bool withPrev = true, bool withNext = true)
        {
	        var concept = GetLiteById(id);

	        if (withPrev)
	        {
				concept.PrevConcept = FindPrevReference(concept.Id);
	        }

	        if (withNext)
	        {
				concept.NextConcept = FindNextReference(concept.Id);
	        }

	        return concept;
        }

        private int? FindNextReference(int conceptId, bool withoutTop = false)
        {
            Concept next = null;
            var concept = GetById(conceptId);
            if (!withoutTop && concept.Children != null && concept.Children.Count > 0)
            {
                next = concept.Children.ElementAt(0);
            }
            else if (concept.NextConcept != null)
            {
                next = GetById(concept.NextConcept.Value);
                withoutTop = false;
            }
            else if (concept.ParentId != null)
            {
                next = GetById(concept.ParentId.Value);
                withoutTop = true;
            }
            if (next == null)
            {
                return null;
            }

            return next.Container != null ? next.Id : FindNextReference(next.Id, withoutTop);
        }

        private int? FindPrevReference(int conceptId, bool withoutTop = false)
        {
            Concept next = null;
            var concept = GetById(conceptId);
            if (!withoutTop && concept.Children != null && concept.Children.Count > 0)
            {
                next = concept.Children.ElementAt(concept.Children.Count - 1);
            }
            else if (concept.PrevConcept != null)
            {
                next = GetById(concept.PrevConcept.Value);
                withoutTop = false;
            }
            else if (concept.ParentId != null)
            {
                next = GetById(concept.ParentId.Value);
                withoutTop = true;
            }
            if (next == null)
            {
                return null;
            }

            return next.Container != null ? next.Id : FindPrevReference(next.Id, withoutTop);
        }

        public IEnumerable<Concept> GetElementsByParentId(int parentId, int authorId)
        {
	        return GetElementsByParentId(parentId).Where(c => c.UserId == authorId);
        }

        public Concept UpdateRootConcept(int id, string name, bool published)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var concept = repositoriesContainer.ConceptRepository.GetById(id);
                concept.Name = name;
                concept.Published = published;
                repositoriesContainer.ConceptRepository.Save(concept);
                repositoriesContainer.ApplyChanges();

                return concept;
            }
        }

        public Concept SaveConcept(Concept concept)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                Concept lastSibling = null;
                if (concept.IsNew)
                {
                    lastSibling = GetLastSibling(concept.ParentId.GetValueOrDefault());
                    if (lastSibling!=null)
                        concept.PrevConcept = lastSibling.Id;
                }

                repositoriesContainer.ConceptRepository.Save(concept);
                if(lastSibling!=null)
                {
                    lastSibling.NextConcept = concept.Id;
                    repositoriesContainer.ConceptRepository.Save(lastSibling);
                }
                repositoriesContainer.ApplyChanges();
                //BindNeighborConcept(concept, source, repositoriesContainer);
                return concept;
            }
        }

        private Concept GetLastSibling(int parentId)
        {
           return GetElementsByParentId(parentId).FirstOrDefault(i=>i.NextConcept==null);
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
            if (!concept.PrevConcept.HasValue && sourceConcept != null && sourceConcept.PrevConcept.HasValue)
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

        private IList<Attachment> ProcessWordAttachmentsIfExist(IList<Attachment> attachments)
        {
            var res = new List<Attachment>();
            var convertor = new WordToPdfConvertor();

            foreach (var attach in attachments)
            {
                var extension = Path.GetExtension(attach.FileName);
                if (string.Compare(extension, ".doc", true) == 0 || string.Compare(extension, ".docx", true) == 0 || string.Compare(extension, ".rtf", true) == 0)
                {
                    var friendlyFileName = Path.GetFileNameWithoutExtension(attach.Name);
                    var sourceFilePath = string.Format("{0}{1}", _storageRootTemp, attach.FileName);
                    res.Add(new Attachment()
                    {
                        AttachmentType = AttachmentType.Document,
                        Id = 0,
                        Name = string.Format("{0}.pdf", friendlyFileName),
                        FileName = convertor.Convert(sourceFilePath)
                    });
                }
                else
                    res.Add(attach);

            }

            return res;
        }

        public Concept SaveConcept(Concept concept, IList<Attachment> attachments)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                //attachments = ProcessWordAttachmentsIfExist(attachments);
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
                if (source == null)
                    InitNeighborConcept(concept, repositoriesContainer);
                BindNeighborConcept(concept, source, repositoriesContainer);
                TryPublishParent(concept.ParentId, repositoriesContainer);
                return concept;
            }
        }

        private void InitNeighborConcept(Concept concept, LmPlatformRepositoriesContainer repositoriesContainer)
        {
            var siblings = repositoriesContainer.ConceptRepository.GetByParentId(concept.ParentId.Value);
            var sibling = siblings.OrderByDescending(t => t.Id).FirstOrDefault(c=>c.Id!=concept.Id);
            if(sibling!=null)
            {
                concept.PrevConcept = sibling.Id;
            }
        }

        private void TryPublishParent(int? parentId, LmPlatformRepositoriesContainer repoContainer)
        {
            if (parentId.HasValue)
            {
                var parent = GetById(parentId.Value);
                var childs = GetElementsByParentId(parent.Id);
                parent.Published = parent.Published ? parent.Published : childs.Any() && childs.All(c => c.Published);
                repoContainer.ConceptRepository.Save(parent);
                repoContainer.ApplyChanges();
                TryPublishParent(parent.ParentId, repoContainer);
            }
            else
                return;
        }

        private void AttachFolderToSection(string folderName, int userId, int subjectId, string sectionName)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var parent = repositoriesContainer.ConceptRepository.GetBy(
                    new Query<Concept>()
                    .AddFilterClause(f => f.SubjectId == subjectId)
                    .AddFilterClause(f => f.UserId == userId)
                    .AddFilterClause(f => string.Compare(f.Name.Trim(), sectionName.Trim(), true) == 0)
                    .Include(c => c.Author)
                    .Include(c => c.Subject));

                var concept = new Concept(folderName, parent.Author, parent.Subject, true, false);
                concept.ParentId = parent.Id;
                repositoriesContainer.ConceptRepository.Save(concept);
            }
        }

        public void AttachFolderToLectSection(string folderName, int userId, int subjectId)
        {
            AttachFolderToSection(folderName, userId, subjectId, LectSectionName);
        }

        public void AttachFolderToLabSection(string folderName, int userId, int subjectId)
        {
            AttachFolderToSection(folderName, userId, subjectId, LabSectionName);
        }

        private string GetGuidFileName()
        {
            return string.Format("P{0}", Guid.NewGuid().ToString("N").ToUpper());
        }

        public Concept CreateRootConcept(string name, int authorId, int subjectId)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        var author = repositoriesContainer.UsersRepository.GetBy(new Query<User>().AddFilterClause(u => u.Id == authorId));
	        var subject = repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>().AddFilterClause(s => s.Id == subjectId));
	        var concept = new Concept(name, author, subject, true, false);

	        repositoriesContainer.ConceptRepository.Save(concept);
	        repositoriesContainer.ApplyChanges();
	        InitBaseChildrens(concept, repositoriesContainer);
	        return repositoriesContainer.ConceptRepository.GetBy(new Query<Concept>().AddFilterClause(c => c.Id == concept.Id));
        }

        private void InitBaseChildrens(Concept parent, LmPlatformRepositoriesContainer repositoriesContainer)
        {
	        var concept1 = new Concept(TitlePageSectionName, parent.Author, parent.Subject, false, false)
	        {
		        ParentId = parent.Id, ReadOnly = true
	        };
	        repositoriesContainer.ConceptRepository.Save(concept1);

	        var concept2 = new Concept(ProgramSectionName, parent.Author, parent.Subject, false, false)
	        {
		        ParentId = parent.Id, ReadOnly = true
	        };
	        repositoriesContainer.ConceptRepository.Save(concept2);

            concept1.NextConcept = concept2.Id;
            concept2.PrevConcept = concept1.Id;

            var concept3 = new Concept(LectSectionName, parent.Author, parent.Subject, true, false)
            {
	            ParentId = parent.Id, ReadOnly = true
            };
            repositoriesContainer.ConceptRepository.Save(concept3);
            InitLectChild(concept3, repositoriesContainer);

            concept2.NextConcept = concept3.Id;
            concept3.PrevConcept = concept2.Id;

            var concept4 = new Concept(LabSectionName, parent.Author, parent.Subject, true, false)
            {
	            ParentId = parent.Id, ReadOnly = true
            };
            repositoriesContainer.ConceptRepository.Save(concept4);
            InitPractChild(concept4, repositoriesContainer);

            concept3.NextConcept = concept4.Id;
            concept4.PrevConcept = concept3.Id;

            var concept5 = new Concept(TestSectionName, parent.Author, parent.Subject, true, true)
            {
	            ParentId = parent.Id, ReadOnly = true
            };
            repositoriesContainer.ConceptRepository.Save(concept5);

            concept5.PrevConcept = concept4.Id;
            concept4.NextConcept = concept5.Id;
            repositoriesContainer.ApplyChanges();
        }

        private void InitLectChild(Concept parent, LmPlatformRepositoriesContainer repositoriesContainer)
        {
            var sub = SubjectManagementService.GetSubject(
	            new Query<Subject>(s => s.Id == parent.SubjectId)
		            .Include(s => s.Lectures));
            Concept prev = null;
            foreach (var item in sub.Lectures.OrderBy(s => s.Order))
            {
	            var concept = new Concept(item.Theme, parent.Author, parent.Subject, true, false)
	            {
		            ParentId = parent.Id, LectureId = item.Id
	            };
	            repositoriesContainer.ConceptRepository.Save(concept);
                if (prev != null)
                {
                    concept.PrevConcept = prev.Id;
                    prev.NextConcept = concept.Id;
                    repositoriesContainer.ConceptRepository.Save(prev);
                    repositoriesContainer.ConceptRepository.Save(concept);
                }
                prev = concept;
            }
        }

        private void InitPractChild(Concept parent, LmPlatformRepositoriesContainer repositoriesContainer)
        {
            Concept prev = null;
            var sub = SubjectManagementService.GetSubject(
	            new Query<Subject>(s => s.Id == parent.SubjectId)
		            .Include(e => e.SubjectModules.Select(x => x.Module))
                    .Include(s => s.Labs)
		            .Include(s => s.Practicals));
            if (sub.SubjectModules.All(m => m.Module.ModuleType != ModuleType.Practical))
            {
	            if (sub.SubjectModules.All(m => m.Module.ModuleType != ModuleType.Labs)) return;

	            foreach (var item in sub.Labs.OrderBy(s => s.Order))
	            {
		            var concept = new Concept(item.Theme, parent.Author, parent.Subject, true, false)
		            {
			            ParentId = parent.Id, LabId = item.Id
		            };
		            repositoriesContainer.ConceptRepository.Save(concept);
		            if (prev != null)
		            {
			            concept.PrevConcept = prev.Id;
			            prev.NextConcept = concept.Id;
			            repositoriesContainer.ConceptRepository.Save(prev);
			            repositoriesContainer.ConceptRepository.Save(concept);
		            }

		            prev = concept;
	            }
            }
            else
            {
	            foreach (var item in sub.Practicals.OrderBy(s => s.Order))
	            {
		            var concept = new Concept(item.Theme, parent.Author, parent.Subject, true, false)
		            {
			            ParentId = parent.Id, PracticalId = item.Id
		            };
		            repositoriesContainer.ConceptRepository.Save(concept);
		            if (prev != null)
		            {
			            concept.PrevConcept = prev.Id;
			            prev.NextConcept = concept.Id;
			            repositoriesContainer.ConceptRepository.Save(prev);
			            repositoriesContainer.ConceptRepository.Save(concept);
		            }

		            prev = concept;
	            }
            }
        }

        private void ResetSiblings(int? prevConcept, int? nextConcept, LmPlatformRepositoriesContainer repositoriesContainer)
        {
            if(prevConcept.HasValue)
            {
                var prevItem = GetById(prevConcept.Value);
                prevItem.NextConcept = nextConcept.HasValue ? nextConcept.Value : (int?)null;
                repositoriesContainer.ConceptRepository.Save(prevItem);
            }
            if(nextConcept.HasValue)
            {
                var nextItem = GetById(nextConcept.Value);
                nextItem.PrevConcept = prevConcept.HasValue ? prevConcept.Value : (int?)null;
                repositoriesContainer.ConceptRepository.Save(nextItem);
            }
            repositoriesContainer.ApplyChanges();
        }

        public bool IsTestModule(string moduleName)
        {
            return string.Compare(TestSectionName, moduleName, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public void Remove(int id, bool removeChildren)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var item = GetById(id);
                var prev = item.PrevConcept;
                var next = item.NextConcept;
                var parentId = item.ParentId;
                repositoriesContainer.ConceptRepository.Remove(id, removeChildren);
                repositoriesContainer.ApplyChanges();
                ResetSiblings(prev, next, repositoriesContainer);
                TryPublishParent(parentId, repositoriesContainer);
            }
        }
    }
}
