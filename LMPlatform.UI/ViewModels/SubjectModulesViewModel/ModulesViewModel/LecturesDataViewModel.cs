using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Helpers;
using Application.Core;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using Newtonsoft.Json;

namespace LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel
{
    public class LecturesDataViewModel
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

        public LecturesDataViewModel()
        {
        }

        public LecturesDataViewModel(Lectures lectures)
        {
            Theme = lectures.Theme;
            LecturesId = lectures.Id;
            Duration = lectures.Duration;
            SubjectId = lectures.SubjectId;
            Order = lectures.Order;
            PathFile = lectures.Attachments;
            Attachments = FilesManagementService.GetAttachments(lectures.Attachments);
        }

        public int Order
        {
            get; 
            set;
        }

        public int LecturesId
        {
            get;
            set;
        }

        public int SubjectId
        {
            get;
            set;
        }

        [Display(Name = "Тема лекции")]
        public string Theme
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

        public LecturesDataViewModel(int id, int subjectId)
        {
            SubjectId = subjectId;
            Attachments = new List<Attachment>();
            if (id != 0)
            {
                var lectures = SubjectManagementService.GetLectures(id);
                Order = lectures.Order;
                Theme = lectures.Theme;
                Duration = lectures.Duration;
                LecturesId = id;
                PathFile = lectures.Attachments;
                Attachments = FilesManagementService.GetAttachments(lectures.Attachments);
            }
        }

        public bool Delete()
        {
            //SubjectManagementService.DeleteNews(new SubjectNews { SubjectId = SubjectId, Id = NewsId });
            return true;
        }

        public bool Save(string attachmentsJson)
        {
            var attachments = JsonConvert.DeserializeObject<List<Attachment>>(attachmentsJson).ToList();
            
            SubjectManagementService.SaveLectures(new Lectures
            {
                SubjectId = SubjectId,
                Duration = Duration,
                Theme = Theme,
                Order = 0,
                Attachments = PathFile,
                Id = LecturesId
            }, attachments);
            return true;
        }
    }
}