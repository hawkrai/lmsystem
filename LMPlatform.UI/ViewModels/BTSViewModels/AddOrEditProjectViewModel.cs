using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core;
using Application.Infrastructure.ProjectManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class AddOrEditProjectViewModel : Controller
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

        public int ProjectId { get; set; }

        [DisplayName("Тема проекта")]
        public string Title { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Создатель")]
        public int CreatorId { get; set; }

        [DisplayName("Создатель")]
        public User Creator { get; set; }

        [DisplayName("Создатель")]
        public string CreatorName { get; set; }

        public AddOrEditProjectViewModel()
        {
            CreatorId = WebSecurity.CurrentUserId;
        }

        public AddOrEditProjectViewModel(Project project)
        {
            Title = project.Title;
            CreatorId = project.CreatorId;
            CreationDate = project.CreationDate;
            ProjectId = project.Id;
        }

        public void UpdateProject(int projectId)
        {
            var project = ProjectManagementService.GetProject(projectId);
            ProjectManagementService.UpdateProject(new Project
            {
                Title = project.Title,
                CreatorId = project.CreatorId,
                CreationDate = project.CreationDate,
                Id = project.Id
            });
        }

        public void SaveProject()
        {
            var creatorId = WebSecurity.CurrentUserId;
            var project = new Project
            {
                Title = Title,
                CreatorId = creatorId,
                CreationDate = DateTime.Today
            };
            ProjectManagementService.SaveProject(project);

            ProjectManagementService.AssingRole(creatorId, project.Id, 1);
        }
    }
}
