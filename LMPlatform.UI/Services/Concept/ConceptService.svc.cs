using Application.Core;
using Application.Infrastructure.ConceptManagement;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.UI.Services.Modules.Concept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebMatrix.WebData;

namespace LMPlatform.UI.Services.Concept
{
    public class ConceptService : IConceptService
    {
        private const String SuccessCode = "200";
        private const String ServerErrorCode = "500";

        private const String SuccessMessage = "Операция выполнена успешно";

        private readonly LazyDependency<IConceptManagementService> _conceptManagementService = new LazyDependency<IConceptManagementService>();

        public IConceptManagementService ConceptManagementService
        {
            get
            {
                return _conceptManagementService.Value;
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



        public ConceptResult SaveRootConcept(string subject, string name, string container)
        {
            try
            {
                var subjectId = Int32.Parse(subject);
                var authorId = WebSecurity.CurrentUserId;

                var root = ConceptManagementService.CreateRootConcept(name, authorId, subjectId);
                return new ConceptResult
                {
                    Concept = new ConceptViewData(root),
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

                return new ConceptResult
                {
                    Concepts = concepts.Select(c => new ConceptViewData(c)).ToList(),
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

        public ConceptResult GetConcepts(String parentId)
        {
            try
            {
                var authorId = WebSecurity.CurrentUserId;
                var parent = Int32.Parse(parentId);

                var concepts = CurrentUserIsLector() ?
                    ConceptManagementService.GetElementsByParentId(authorId, parent) :
                    ConceptManagementService.GetElementsByParentId(parent).Where(c => c.Published);

                var concept = ConceptManagementService.GetById(parent);

                return new ConceptResult
                {
                    Concepts = concepts.Select(c => new ConceptViewData(c)).ToList(),
                    Concept = new ConceptViewData(concept),
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

        public ConceptViewData GetConceptTree(String elementId)
        {
            try
            {
                var parentId = Int32.Parse(elementId);

                var tree = ConceptManagementService.GetTreeConceptByElementId(parentId);

                return new ConceptViewData(tree, true);

            }
            catch (Exception)
            {
                return null;
            }
        }

        public AttachViewData GetNextConceptData(String elementId)
        {
            var id = Int32.Parse(elementId);
            var concept = ConceptManagementService.GetById(id);
            return GetNeighborConceptData(concept.NextConcept.GetValueOrDefault());
        }

        public AttachViewData GetPrevConceptData(String elementId)
        {
            var id = Int32.Parse(elementId);
            var concept = ConceptManagementService.GetById(id);
            return GetNeighborConceptData(concept.PrevConcept.GetValueOrDefault());
        }

        private AttachViewData GetNeighborConceptData(Int32 neighborId)
        {
            var neighbor = ConceptManagementService.GetById(neighborId);
            if (neighbor == null)
                return new AttachViewData(0, String.Empty, null);
            var att = FilesManagementService.GetAttachments(neighbor.Container).FirstOrDefault();
            return new AttachViewData(neighbor.Id, neighbor.Name, att);
        }
    }
}
