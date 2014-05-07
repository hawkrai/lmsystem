using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core;
using Application.Infrastructure.BugManagement;
using Application.Infrastructure.ProjectManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class AddOrEditBugViewModel : Controller
    {
        private readonly LazyDependency<IBugsRepository> _bugsRepository = new LazyDependency<IBugsRepository>();
        private readonly LazyDependency<IBugManagementService> _bugManagementService = new LazyDependency<IBugManagementService>();

        public IBugsRepository BugsRepository
        {
            get
            {
                return _bugsRepository.Value;
            }
        }

        public IBugManagementService BugManagementService
        {
            get
            {
                return _bugManagementService.Value;
            }
        }

        public int BugId { get; set; }

        [Required(ErrorMessage = "Поле Название обязательно для заполнения")]
        [StringLength(100, ErrorMessage = "Название не может иметь размер больше 100 символов")]
        [DataType(DataType.Text)]
        [DisplayName("Название")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Поле Описание обязательно для заполнения")]
        [StringLength(150, ErrorMessage = "Описание не может иметь размер больше 150 символов")]
        [DataType(DataType.Text)]
        [DisplayName("Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле Шаги выполнения обязательно для заполнения")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Шаги выполнения")]
        public string Steps { get; set; }

        [Required(ErrorMessage = "Поле Ожидаемый результат обязательно для заполнения")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Ожидаемый результат")]
        public string ExpectedResult { get; set; }

        [DisplayName("Симптом")]
        public int SymptomId { get; set; }

        [DisplayName("Проект")]
        public int ProjectId { get; set; }

        [DisplayName("Важность")]
        public int SeverityId { get; set; }

        [DisplayName("Статус")]
        public int StatusId { get; set; }

        public int CreatorId { get; set; }

        public DateTime ReportingDate { get; set; }

        public DateTime ModifyingDate { get; set; }

        public AddOrEditBugViewModel()
        {
            CreatorId = WebSecurity.CurrentUserId;
        }

        public AddOrEditBugViewModel(int bugId)
        {
            BugId = bugId;

            if (bugId != 0)
            {
                var bug = BugManagementService.GetBug(bugId);
                BugId = bugId;
                CreatorId = bug.ReporterId;
                ProjectId = bug.ProjectId;
                SeverityId = bug.SeverityId;
                StatusId = bug.StatusId;
                SymptomId = bug.SymptomId;
                Steps = bug.Steps;
                ExpectedResult = bug.ExpectedResult;
                Description = bug.Description;
                Summary = bug.Summary;
                ReportingDate = bug.ReportingDate;
                ModifyingDate = bug.ModifyingDate;
            }
        }

        public IList<SelectListItem> GetStatusNames()
        {
            var statuses = new LmPlatformModelsContext().BugStatuses.ToList();
            return statuses.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();
        }

        public IList<SelectListItem> GetSeverityNames()
        {
            var severities = new LmPlatformModelsContext().BugSeverities.ToList();
            return severities.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();
        }

        public IList<SelectListItem> GetSymptomNames()
        {
            var symptoms = new LmPlatformModelsContext().BugSymptoms.ToList();
            return symptoms.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();
        }

        public IList<SelectListItem> GetProjectNames()
        {
            var projects = new ProjectManagementService().GetProjects();

            return projects.Select(e => new SelectListItem
            {
                Text = e.Title,
                Value = e.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();
        }

        public void Save(int reporterId)
        {
            var bug = new Bug
            {
                Id = BugId,
                ReporterId = reporterId,
                ProjectId = ProjectId,
                SeverityId = SeverityId,
                StatusId = StatusId,
                SymptomId = SymptomId,
                Steps = Steps,
                ExpectedResult = ExpectedResult,
                Description = Description,
                Summary = Summary,
                ReportingDate = DateTime.Today,
                ModifyingDate = DateTime.Today
            };

            BugManagementService.SaveBug(bug);
        }
    }
}