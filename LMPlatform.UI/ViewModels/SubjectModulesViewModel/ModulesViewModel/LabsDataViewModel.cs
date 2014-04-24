namespace LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Application.Core;
    using Application.Infrastructure.FilesManagement;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.Models;

    using Newtonsoft.Json;

    public class LabsDataViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IFilesManagementService> _filesManagementService = new LazyDependency<IFilesManagementService>();

        private IFilesManagementService FilesManagementService
        {
            get
            {
                return _filesManagementService.Value;
            }
        }

        private ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

        public LabsDataViewModel()
        {
        }

        public LabsDataViewModel(Labs labs)
        {
            Theme = labs.Theme;
            LabsId = labs.Id;
            Duration = labs.Duration;
            SubjectId = labs.SubjectId;
            Order = labs.Order;
            PathFile = labs.Attachments;
            ShortName = labs.ShortName;
            Attachments = FilesManagementService.GetAttachments(labs.Attachments);
        }

        public int Order
        {
            get; 
            set;
        }

        public int LabsId
        {
            get;
            set;
        }

        public int SubjectId
        {
            get;
            set;
        }

        [Display(Name = "Название лабораторной работы")]
        public string Theme
        {
            get;
            set;
        }

        [Display(Name = "Короткое название")]
        public string ShortName
        {
            get;
            set;
        }

        [Display(Name = "Количество часов (1-99)")]
        public int Duration
        {
            get; 
            set;
        }

        public string PathFile
        {
            get;
            set;
        }

        public IList<Attachment> Attachments
        {
            get;
            set;
        }

        //public LabsDataViewModel(int id, int subjectId)
        //{
        //    SubjectId = subjectId;
        //    Attachments = new List<Attachment>();
        //    if (id != 0)
        //    {
        //        var labs = SubjectManagementService.GetLabs(id);
        //        Order = labs.Order;
        //        Theme = labs.Theme;
        //        Duration = labs.Duration;
        //        LabsId = id;
        //        ShortName = labs.ShortName;
        //        PathFile = labs.Attachments;
        //        Attachments = FilesManagementService.GetAttachments(labs.Attachments);
        //    }
        //}

        //public bool Delete()
        //{
        //    //SubjectManagementService.DeleteNews(new SubjectNews { SubjectId = SubjectId, Id = NewsId });
        //    return true;
        //}

        //public bool Save(string attachmentsJson)
        //{
        //    var attachments = JsonConvert.DeserializeObject<List<Attachment>>(attachmentsJson).ToList();

        //    SubjectManagementService.SaveLabs(new Labs
        //    {
        //        SubjectId = SubjectId,
        //        Duration = Duration,
        //        Theme = Theme,
        //        Order = 0,
        //        ShortName = ShortName,
        //        Attachments = PathFile,
        //        Id = LabsId
        //    }, attachments);
        //    return true;
        //} 
    }
}