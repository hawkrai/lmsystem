using System;
using System.ComponentModel.DataAnnotations;
using Application.Core;
using Application.Infrastructure.ProjectManagement;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class ProjectsViewModel
    {
        private readonly LazyDependency<IProjectsRepository> _projectsRepository = new LazyDependency<IProjectsRepository>();
        private readonly LazyDependency<IProjectManagementService> _projectManagementService = new LazyDependency<IProjectManagementService>(); 

        public IProjectsRepository ProjectsRepository
        {
            get
            {
                return _projectsRepository.Value;
            }
        }

        public IProjectManagementService ProjectManagementService
        {
            get
            {
                return _projectManagementService.Value;
            }
        }

        [DataType(DataType.Text)]
        [Display(Name = "Тема проекта")]
        public string Title { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Создатель")]
        public User Creator { get; set; }

        [Display(Name = "Избранный")]
        public bool IsChosen { get; set; }

        public void SaveProject()
        {
            var creatorId = WebSecurity.CurrentUserId;
            ProjectManagementService.SaveProject(new Project
                {
                    Title = Title,
                    CreatorId = creatorId,
                    CreationDate = DateTime.Today,
                    IsChosen = IsChosen
                });
        }
    }
}
