using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Application.Core;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.SubjectViewModels
{
    public class SubjectEditViewModel
    {
        private readonly LazyDependency<IModulesManagementService> _modulesManagementService = new LazyDependency<IModulesManagementService>();
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

        public IModulesManagementService ModulesManagementService
        {
            get
            {
                return _modulesManagementService.Value;
            }
        }

        public ICollection<ModulesViewModel> Modules
        {
            get; 
            set;
        }

        [Required(ErrorMessage = "Название предмета не может быть пустым")]
        [Display(Name = "Название предмета", Description = "Введите название предмета. Название будет использоваться при отображени предмета.")]
        public string DisplayName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Аббривиатура не может быть пустой")]
        [Display(Name = "Аббривиатура", Description = "Аббривиатура представляет собой сокрощенное название предмета. Например: Базы данных - БД")]
        public string ShortName
        {
            get;
            set;
        }

        public string Title
        {
            get; 
            set;
        }

        public int SubjectId
        {
            get;
            set;
        }

        public SubjectEditViewModel()
        {
            Modules = new Collection<ModulesViewModel>();
        }

        public SubjectEditViewModel(int subjectId)
        {
            SubjectId = subjectId;
            Title = SubjectId == 0 ? "Создание предмета" : "Редактирование предмета"; 
            Modules = ModulesManagementService.GetModules().Select(e => new ModulesViewModel(e)).ToList();
        }

        public void Save(int userId)
        {
            var subject = new Subject
            {
                Id = SubjectId,
                Name = DisplayName,
                ShortName = ShortName,
                SubjectModules = new Collection<SubjectModule>(),
                SubjectLecturers = new Collection<SubjectLecturer>()
            };

            foreach (var module in Modules)
            {
                if (module.Checked)
                {
                    subject.SubjectModules.Add(new SubjectModule
                    {
                        ModuleId = module.ModuleId,
                        SubjectId = SubjectId
                    });
                }
            }

            subject.SubjectLecturers.Add(new SubjectLecturer
            {
                SubjectId = SubjectId,
                LecturerId = userId
            });

            SubjectManagementService.SaveSubject(subject);
        }
    }
}