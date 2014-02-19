using System.ComponentModel.DataAnnotations;
using Application.Core;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel
{
    public class LecturesDataViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
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
    }
}