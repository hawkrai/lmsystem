using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
        private readonly LazyDependency<IProjectsRepository> _projectsRepository =
            new LazyDependency<IProjectsRepository>();

        private readonly LazyDependency<IProjectManagementService> _projectManagementService =
            new LazyDependency<IProjectManagementService>();

        public IProjectsRepository ProjectsRepository
        {
            get { return _projectsRepository.Value; }
        }

        public IProjectManagementService ProjectManagementService
        {
            get { return _projectManagementService.Value; }
        }

        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Поле Тема проекта обязательно для заполнения")]
        [StringLength(100, ErrorMessage = "Тема проекта должна быть не менее 3 символов.", MinimumLength = 3)]
        
        [DataType(DataType.Text)]
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

        public AddOrEditProjectViewModel(int projectId)
        {
            ProjectId = projectId;

            if (projectId != 0)
            {
                var project = ProjectManagementService.GetProject(projectId);
                CreatorId = project.CreatorId;
                CreationDate = project.CreationDate;
                ProjectId = project.Id;
                Title = project.Title;
            }
        }

        public void Update(int projectId)
        {
            ProjectManagementService.UpdateProject(new Project
            {
                Id = projectId,
                Title = Title
            });
        }

        public void Save(int creatorId)
        {
            var project = new Project
            {
                Id = ProjectId,
                Title = Title,
                CreatorId = creatorId,
                CreationDate = DateTime.Today
            };

            ProjectManagementService.SaveProject(project);

            if (ProjectId == 0)
            {
                ProjectManagementService.AssingRole(new ProjectUser
                {
                    UserId = creatorId,
                    ProjectId = project.Id,
                    ProjectRoleId = 1
                });
            }
        }
    }
}
