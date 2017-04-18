using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Application.Core;
using Application.Infrastructure.ProjectManagement;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    //public class ProjectValidator : AbstractValidator<Project>
    //{
    //    public ProjectValidator()
    //    {
    //        RuleFor(x => x.Title).Must(BeUniqueTitle).WithMessage("Такой проект уже существует");
    //    }
    //    private bool BeUniqueTitle(string title)
    //    {
    //        var context = new ProjectManagementService().GetProjects();
    //        if (context.SingleOrDefault(x => x.Title == title) == null)
    //        {
    //            return true;
    //        }
    //        return false;
    //    }
    //}
    //[Validator(typeof(ProjectValidator))]
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
        [StringLength(300, ErrorMessage = "Тема проекта должна быть не менее 3 символов и не более 300.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [DisplayName("Тема проекта")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Описание проекта не должно превышать 1000 символов.")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Описание проекта")]
        public string Details { get; set; }

        [DisplayName("Дата изменения")]
        public DateTime DateOfChange { get; set; }

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
                Details = project.Details;
                DateOfChange = project.DateOfChange;
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
                Details = Details,
                CreatorId = creatorId,
                DateOfChange = DateTime.Today
            };

            //if (project.Id == 0)
            //{
            //    if (new ProjectManagementService().GetProjects().Count(e => e.Title == project.Title) != 0)
            //    {
            //    }
            //}
            //else
            //{
            //    if (
            //        new ProjectManagementService().GetProjects()
            //            .Count(e => e.Title == project.Title && e.Id != project.Id) != 0)
            //    {
            //    }
            //    else
            //    {
                    ProjectManagementService.SaveProject(project);

                    if (ProjectId == 0)
                    {
                        ProjectManagementService.AssingRole(new ProjectUser
                        {
                            UserId = creatorId,
                            ProjectId = project.Id,
                            ProjectRoleId = 3
                        });
                    }

            //    }
            //}
        }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (ProjectId == 0)
        //    {
        //        if (new ProjectManagementService().GetProjects().Count(e => e.Title == Title) != 0)
        //        {
        //            ModelState.AddModelError("Title", "Введите корректную тему проекта");
        //        }
        //    }
        //    else
        //    {
        //        if (
        //            new ProjectManagementService().GetProjects()
        //                .Count(e => e.Title == Title && e.Id != ProjectId) != 0)
        //        {
        //            yield return new ValidationResult("Введите корректную тему проекта");
        //        }
        //    }
        //}
    }
}
