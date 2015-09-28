using Application.Core;
using Application.Infrastructure.ConceptManagement;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMPlatform.UI.ViewModels.ComplexMaterialsViewModel
{
    public class AddOrEditConceptViewModel : AddOrEditRootConceptViewModel
    {
        private readonly LazyDependency<IFilesManagementService> _filesManagementService =
           new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get { return _filesManagementService.Value; }
        }


        public String Container { get; set; }
        public String FileData { get; set; }
        public Boolean IsGroup { get; set; }

        [DisplayName("Родительский элемент")]
        public Int32 ParentId { get; set; }

        [DisplayName("Следующий")]
        public Int32? Next { get; set; }

        [DisplayName("Предыдущий")]
        public Int32? Prev { get; set; }

        protected IList<Attachment> Attachments { get; set; }

        public AddOrEditConceptViewModel(Int32 currentAuthorId, Int32 id)
            : base(currentAuthorId, id)
        {
            if (!IsNew())
            {
                Container = SourceConcept.Container;
                if (!String.IsNullOrEmpty(Container))
                    Attachments = FilesManagementService.GetAttachments(Container);
                ParentId = SourceConcept.ParentId.GetValueOrDefault();
                IsGroup = SourceConcept.IsGroup;
                Prev = SourceConcept.PrevConcept;
                Next = SourceConcept.NextConcept;
            }
            
            
            //Attachments = new List<Attachment>();
        }

        public AddOrEditConceptViewModel(Int32 currentAuthorId, Int32 id, Int32 parentId)
            : this(currentAuthorId, id)
        {
            ParentId = parentId;
            if(IsNew())
            {
                var c = ConceptManagementService.GetById(ParentId);
                SelectedSubjectId = c.SubjectId;
            }
        }

        public AddOrEditConceptViewModel():base()
        {
        }

        public String GetRelativePathForActiveAttachment()
        {
            var att = Attachments.FirstOrDefault();
            if(att==null)
                return String.Empty;
            var uploadFolder = "UploadedFiles";
            return String.Format("/{0}/{1}/{2}", uploadFolder, att.PathName, att.FileName);
        }

        public void SetAttachments(IList<Attachment> attachments)
        {
            Attachments = attachments;
        }

        public IList<Attachment> GetAttachments()
        {
            return Attachments != null ? Attachments : new List<Attachment>();
        }

        public override void Save()
        {
            InitSourceConcept();
            if (!IsGroup)
                ConceptManagementService.SaveConcept(SourceConcept, GetAttachments());
            else                
                ConceptManagementService.SaveConcept(SourceConcept);
            
        }

        private void InitSourceConcept()
        {
            SourceConcept = new Concept()
            {
                Id = this.Id,
                Name = this.Name,
                Container = this.Container,
                IsGroup = this.IsGroup,
                Published = this.Published,
                ReadOnly = this.ReadOnly,
                SubjectId = this.SelectedSubjectId,
                UserId = this.Author,
                ParentId = this.ParentId,
                PrevConcept = this.Prev,
                NextConcept = this.Next
            };
            
        }
    }


    public class AddOrEditRootConceptViewModel 
    {

        private readonly LazyDependency<IConceptManagementService> _conceptManagementService =
            new LazyDependency<IConceptManagementService>();

        public IConceptManagementService ConceptManagementService
        {
            get { return _conceptManagementService.Value; }
        }

        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService =
            new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get { return _subjectManagementService.Value; }
        }

        public AddOrEditRootConceptViewModel(Int32 currentAuthorId, Int32 id)
        {
            Author = currentAuthorId;
            Id = id;
            if (!IsNew())
            {
                SelectedSubjectId = SourceConcept.SubjectId;
                Name = SourceConcept.Name;
                Published = SourceConcept.Published;
                ReadOnly = SourceConcept.ReadOnly;
            }
        }
        
        public AddOrEditRootConceptViewModel()
        {
        }

        public IList<SelectListItem> GetBroElements(Int32 author, Int32 parentId)
        {
            var brothers = ConceptManagementService.GetElementsByParentId(author, parentId).Where(c=>c.Id!=this.Id);
            var res = new List<SelectListItem>();
            foreach (var b in brothers)
            {
                res.Add(new SelectListItem()
                {
                    Text = b.Name,
                    Value = b.Id.ToString(CultureInfo.InvariantCulture),
                    Selected = !IsNew() && b.Id == Id
                });
            }
            return res;
        }

        public IList<SelectListItem> GetSubjects(Int32 currentAuthorId)
        {
            var currentSubjects = SubjectManagementService.GetUserSubjects(currentAuthorId).Where(e => !e.IsArchive);
            var res = new List<SelectListItem>();
            foreach (var sub in currentSubjects)
            {
                res.Add(new SelectListItem()
                    {
                        Text = sub.Name,
                        Value = sub.Id.ToString(CultureInfo.InvariantCulture),
                        Selected = !IsNew() && sub.Id==Id
                    });
            }
            return res;
        }

        public Boolean IsNew()
        {
            return Id == 0;
        }

        public virtual void Save()
        {
            if (IsNew())
                ConceptManagementService.CreateRootConcept(this.Name, this.Author, this.SelectedSubjectId);
            else
                ConceptManagementService.UpdateRootConcept(this.Id, this.Name);
        }

        public Int32 Id { get; set; }

        [Required(ErrorMessage = "Поле Название обязательно для заполнения")]
        [StringLength(250, ErrorMessage = "Название должно быть не менее 3 символов и не более 250.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [DisplayName("Название")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Поле Предмет обязательно для заполнения")]
        [DisplayName("Предмет")]
        public Int32 SelectedSubjectId { get; set; }

        public Int32 Author { get; set; }

        public Boolean ReadOnly { get; set; }

        [DisplayName("Опубликован")]
        public Boolean Published { get; set; }


        private Concept _sourceConcept;
        protected Concept SourceConcept 
        { 
            get
            {
                if (IsNew())
                    return _sourceConcept;
                else
                {
                    if (_sourceConcept == null)
                        _sourceConcept = ConceptManagementService.GetById(Id);
                    return _sourceConcept;
                }
            }
            set
            {
                _sourceConcept = value;
            }
 
        }
    }
}