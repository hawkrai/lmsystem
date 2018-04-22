using Application.Core;
using Application.Infrastructure.ConceptManagement;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.SubjectManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.UI.Services.Modules.Concept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebMatrix.WebData;
using LMPlatform.Models;
using Application.Infrastructure.WatchingTimeManagement;
using Application.Infrastructure.StudentManagement;
using LMPlatform.UI.Services.Modules;
using Application.Infrastructure.KnowledgeTestsManagement;

namespace LMPlatform.UI.Services.Concept
{
    public class ConceptService : IConceptService
    {
        private const String SuccessCode = "200";
        private const String ServerErrorCode = "500";

        private const String SuccessMessage = "Операция выполнена успешно";

        private readonly LazyDependency<IStudentManagementService> _studentManagementService = new LazyDependency<IStudentManagementService>();
        private readonly LazyDependency<IConceptManagementService> _conceptManagementService = new LazyDependency<IConceptManagementService>();
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IWatchingTimeService> _watchingTimeService = new LazyDependency<IWatchingTimeService>();
        private readonly LazyDependency<IQuestionsManagementService> _questionsManagementService = new LazyDependency<IQuestionsManagementService>();
        private readonly LazyDependency<ITestPassingService> _testPassingService = new LazyDependency<ITestPassingService>();

        public ITestPassingService TestPassingService
        {
            get
            {
                return _testPassingService.Value;
            }
        }

        public IQuestionsManagementService QuestionsManagementService
        {
            get
            {
                return _questionsManagementService.Value;
            }
        }

        public IConceptManagementService ConceptManagementService
        {
            get
            {
                return _conceptManagementService.Value;
            }
        }

        public IStudentManagementService StudentManagementService
        {
            get
            {
                return _studentManagementService.Value;
            }
        }

        public IWatchingTimeService WatchingTimeService
        {
            get
            {
                return _watchingTimeService.Value;
            }
        }

        private readonly LazyDependency<IUsersManagementService> _usersManagementService = new LazyDependency<IUsersManagementService>();

        public IUsersManagementService UsersManagementService
        {
            get
            {
                return _usersManagementService.Value;
            }
        }

        private readonly LazyDependency<IFilesManagementService> _filesManagementService =
           new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get { return _filesManagementService.Value; }
        }

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

        public ConceptResult AttachSiblings(string source, string left, string right)
        {
            try
            {
                var sourceId = Int32.Parse(source);
                var leftId = 0;
                var rightId = 0;
                Int32.TryParse(left, out leftId);
                Int32.TryParse(right, out rightId);

                var concept = ConceptManagementService.AttachSiblings(sourceId, rightId, leftId);

                return new ConceptResult
                {
                    Concept = new ConceptViewData(concept),
                    Message = SuccessMessage,
                    Code = SuccessCode,
                    SubjectName = concept.Subject.Name
                };

            }
            catch (Exception ex)
            {
                
                return new ConceptResult
                {
                    Message = ex.Message,
                    Code = ServerErrorCode
                };
            }
        }

        public ConceptResult SaveRootConcept(string subject, string name, string container)
        {
            try
            {
                var subjectId = Int32.Parse(subject);
                var authorId = WebSecurity.CurrentUserId;

                var root = ConceptManagementService.CreateRootConcept(name, authorId, subjectId);
                var subj = SubjectManagementService.GetSubject(subjectId);
                return new ConceptResult
                {
                    Concept = new ConceptViewData(root),
                    Message = SuccessMessage,
                    SubjectName = subj.Name,
                    Code = SuccessCode
                };
            }
            catch (Exception ex)
            {
                return new ConceptResult
                {
                    Message = ex.Message,
                    Code = ServerErrorCode
                };
            }
        }


        private Boolean CurrentUserIsLector()
        {
            return UsersManagementService.CurrentUser.Membership.Roles.Any(r => r.RoleName.Equals("lector"));
        }

        public ConceptResult GetRootConcepts(String subjectId)
        {
            try
            {
                var subject = 0;
                var valid = Int32.TryParse(subjectId, out subject);
                var authorId = WebSecurity.CurrentUserId;

                var concepts = CurrentUserIsLector()  ?
                    ConceptManagementService.GetRootElements(authorId) : (valid ? 
                    ConceptManagementService.GetRootElementsBySubject(subject).Where(c=>c.Published) : new List<LMPlatform.Models.Concept>());

                if(valid)
                    concepts = concepts.Where(c => c.SubjectId == subject);
                var subj = SubjectManagementService.GetSubject(subject);


                return new ConceptResult
                {
                    Concepts = concepts.Select(c => new ConceptViewData(c)).ToList(),
                    Message = SuccessMessage,
                    SubjectName = subj.Name,
                    Code = SuccessCode
                };
            }
            catch (Exception ex)
            {

                return new ConceptResult
                {
                    Message = ex.Message,
                    Code = ServerErrorCode
                };
            }
        }

        public ConceptResult GetConcepts(String parentId)
        {
            try
            {
                var authorId = WebSecurity.CurrentUserId;
                var parent = Int32.Parse(parentId);

                var concepts = CurrentUserIsLector() ?
                    ConceptManagementService.GetElementsByParentId(authorId, parent) :
                    ConceptManagementService.GetElementsByParentId(parent);

                var concept = ConceptManagementService.GetById(parent);

                return new ConceptResult
                {
                    Concepts = concepts.Select(c => new ConceptViewData(c)).ToList().SortDoubleLinkedList(),
                    Concept = new ConceptViewData(concept),
                    SubjectName = concept.Subject.Name,
                    Message = SuccessMessage,
                    Code = SuccessCode
                };
            }
            catch (Exception ex)
            {

                return new ConceptResult
                {
                    Message = ex.Message,
                    Code = ServerErrorCode
                };
            }
        }

        public ConceptResult Remove(String id)
        {
            try
            {
                var conceptId = Int32.Parse(id);
                var source = ConceptManagementService.GetById(conceptId);
                var canDelete = source != null && source.Author.Id == WebSecurity.CurrentUserId;
                if (canDelete)
                {
                    ConceptManagementService.Remove(conceptId, source.IsGroup);
                }

                return new ConceptResult
                {
                    Message = SuccessMessage,
                    Code = SuccessCode,
                    SubjectName = source.Subject.Name
                };
            }
            catch (Exception ex)
            {
                return new ConceptResult
                {
                    Message = ex.Message,
                    Code = ServerErrorCode
                };
            }
        }

        public ConceptViewData GetConceptTree(String elementId)
        {
            try
            {
                var parentId = Int32.Parse(elementId);

                var tree = ConceptManagementService.GetTreeConceptByElementId(parentId);

                return new ConceptViewData(tree, true);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AttachViewData GetNextConceptData(String elementId)
        {
            var id = Int32.Parse(elementId);
            var concept = ConceptManagementService.GetByIdFixed(id);
            return GetNeighborConceptData(concept.NextConcept.GetValueOrDefault());
        }

        public AttachViewData GetPrevConceptData(String elementId)
        {
            var id = Int32.Parse(elementId);
            var concept = ConceptManagementService.GetByIdFixed(id);
            return GetNeighborConceptData(concept.PrevConcept.GetValueOrDefault());
        }

        public MonitoringData GetConceptViews(int conceptId, int groupId)
        {
            var concept = ConceptManagementService.GetById(conceptId);
            var list = WatchingTimeService.GetAllRecords(conceptId);
            var viewRecords = new List<ViewsWorm>();
            foreach (var item in list)
            {
                var student = StudentManagementService.GetStudent(item.UserId);
                if (student != null && student.GroupId == groupId)
                    viewRecords.Add(new ViewsWorm { Name = UsersManagementService.GetUser(item.UserId).FullName, Seconds = item.Time });
            }
            var views = viewRecords.OrderBy(x => x.Name).ToList();
            var estimated = WatchingTimeService.GetEstimatedTime(concept.Container);
            return new MonitoringData() { Views = views, Estimated = estimated };
        }

        public class MonitoringData
        {
            public List<ViewsWorm> Views { get; set; }
            public int Estimated { get; set; }
        }
        public class ViewsWorm
        {
            public string Name { get; set; }
            public int Seconds { get; set; }
        }
        private AttachViewData GetNeighborConceptData(Int32 neighborId)
        {
            var neighbor = ConceptManagementService.GetById(neighborId);
            if (neighbor == null)
                return new AttachViewData(0, String.Empty, null);
            var att = FilesManagementService.GetAttachments(neighbor.Container).FirstOrDefault();
            return new AttachViewData(neighbor.Id, neighbor.Name, att);
        }

        public ConceptViewData GetConcept(String elementId)
        {
            return new ConceptViewData(ConceptManagementService.GetById(Convert.ToInt32(elementId)));
        }

        public ConceptPageTitleData GetConceptTitleInfo(string subjectId)
        {
            var subject = 0;
            var valid = Int32.TryParse(subjectId, out subject);
            var lecturer = SubjectManagementService.GetSubject(subject).SubjectLecturers.FirstOrDefault().Lecturer;
            var subj = SubjectManagementService.GetSubject(subject);
            return new ConceptPageTitleData()
            {
                Lecturer = new LectorViewData(lecturer, true),
                Subject = new Modules.Parental.SubjectViewData(subj)
            };
        }

        public IList<ConceptResult> GetRecomendations(int rootConceptId)
        {
            IList<ConceptResult> result = new List<ConceptResult>();
            try
            {
                var concepts = ConceptManagementService.GetTreeConceptByElementId(rootConceptId).GetAllChildren().Where(x => x.IsGroup);
                foreach(var concept in concepts)
                {
                    var questions = QuestionsManagementService.GetQuestionsByConceptId(concept.Id);
                    if(questions != null)
                    {
                        foreach (var question in questions)
                        {
                            var points = TestPassingService.GetPointsForQuestion(4473, question.Id);
                            if(points == 0)
                            {
                                result.Add(new ConceptResult
                                {
                                    Concept = new ConceptViewData(concept)
                                });
                                break;
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return result;
        }
    }
}
