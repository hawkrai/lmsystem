using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class AddBugViewModel : Controller
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

        //[DisplayName("Проект")]
        //public string Project { get; set; }
        [DisplayName("Название")]
        public string Summary { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        [DisplayName("Шаги выполнения")]
        public string Steps { get; set; }

        [DisplayName("Симптом")]
        public int SymptomId { get; set; }

        [DisplayName("Проект")]
        public int ProjectId { get; set; }

        [DisplayName("Важность")]
        public int SeverityId { get; set; }

        [DisplayName("Статус")]
        public int StatusId { get; set; }

        public int CreatorId { get; set; }

        //public string Symptom { get; set; }
        //public string Severity { get; set; }
        //public string Status { get; set; }
        public AddBugViewModel()
        {
            CreatorId = WebSecurity.CurrentUserId;
        }

        public AddBugViewModel(Bug bug)
        {
            BugId = bug.Id;
            ProjectId = bug.ProjectId;
            SeverityId = bug.SeverityId;
            StatusId = bug.StatusId;
            SymptomId = bug.SymptomId;
            Steps = bug.Steps;
            Description = bug.Description;
            Summary = bug.Summary;
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

        public void SaveBug()
        {
            var reporterId = WebSecurity.CurrentUserId;
            var bug = new Bug
            {
                ReporterId = reporterId,
                ProjectId = ProjectId,
                SeverityId = SeverityId,
                StatusId = StatusId,
                SymptomId = SymptomId,
                Steps = Steps,
                Description = Description,
                Summary = Summary,
                ReportingDate = DateTime.Today,
                ModifyingDate = DateTime.Today
            };
            BugManagementService.SaveBug(bug);
        }
    }
}