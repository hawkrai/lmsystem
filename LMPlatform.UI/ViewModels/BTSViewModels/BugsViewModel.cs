using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;
using Application.Core;
using Application.Infrastructure.BugManagement;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using LMPlatform.Models.BTS;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class BugsViewModel
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
        
        public int ProjectId { get; set; }

        [Display(Name = "Кем добавлена")]
        public User Reporter { get; set; }

        [Display(Name = "Содержание")]
        public string Summary { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Шаги выполнения")]
        public string Steps { get; set; }

        [Display(Name = "Дата документирования")]
        public DateTime ReportingDate { get; set; }

        [Display(Name = "Дата последнего изменения")]
        public DateTime ModifyingDate { get; set; }

        public int SymptomId { get; set; }

        public int SeverityId { get; set; }

        public int StatusId { get; set; }

        public int ReporterId { get; set; }

        public Project Project { get; set; }

        public BugSymptom Symptom { get; set; }

        public BugSeverity Severity { get; set; }

        public BugStatus Status { get; set; }

        public IList<BugSymptom> GetSymptomNames()
        {
            var symptoms = BugManagementService.GetBugs();
            return symptoms.Select(e => new BugSymptom()).ToList();
        }

        public IList<BugSymptom> GetStatusNames()
        {
            var statuses = BugManagementService.GetBugs();
            return statuses.Select(e => new BugSymptom()).ToList();
        }

        public IList<BugSymptom> GetSeverityNames()
        {
            var severity = BugManagementService.GetBugs();
            return severity.Select(e => new BugSymptom()).ToList();
        }
    }
}