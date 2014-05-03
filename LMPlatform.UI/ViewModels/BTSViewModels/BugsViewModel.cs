using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.BugManagement;
using Application.Infrastructure.ProjectManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using LMPlatform.Models.BTS;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class BugsViewModel
    {
        private static LmPlatformModelsContext context = new LmPlatformModelsContext();
        
        public int ProjectId { get; set; }

        [Display(Name = "Кем добавлена")]
        public string ReporterName { get; set; }

        [Display(Name = "Название")]
        public string Summary { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Шаги выполнения")]
        public string Steps { get; set; }

        [Display(Name = "Ожидаемый результат")]
        public string ExpectedResult { get; set; }

        [DisplayName("Важность")]
        public string Severity { get; set; }

        [DisplayName("Статус")]
        public string Status { get; set; }

        [DisplayName("Симптом")]
        public string Symptom { get; set; }

        [DisplayName("Проект")]
        public string Project { get; set; }

        [Display(Name = "Дата документирования")]
        public string ReportingDate { get; set; }

        [Display(Name = "Дата последнего изменения")]
        public string ModifyingDate { get; set; }

        public int SymptomId { get; set; }

        public int SeverityId { get; set; }

        public int StatusId { get; set; }

        public int ReporterId { get; set; }

        public int BugId { get; set; }

        public BugsViewModel(int id)
        {
            var model = new BugManagementService().GetBug(id);
            BugId = id;
            SetParams(model);
        }

        public void SetParams(Bug model)
        {
            Steps = model.Steps;
            ExpectedResult = model.ExpectedResult;
            Symptom = GetSymptomName(model.SymptomId);
            ReporterName = GetReporterName(model.ReporterId);
            ReportingDate = model.ReportingDate.ToShortDateString();
            Summary = model.Summary;
            Description = model.Description;
            Severity = GetSeverityName(model.SeverityId);
            Status = GetStatusName(model.StatusId);
            Project = GetProjectTitle(model.ProjectId);
            ModifyingDate = model.ModifyingDate.ToShortDateString();
            ReportingDate = model.ReportingDate.ToShortDateString();
        }

        public static string GetProjectTitle(int id)
        {
            var projectManagementService = new ProjectManagementService();
            var project = projectManagementService.GetProject(id);
            return project.Title;
        }

        public static string GetReporterName(int id)
        {
            var context = new LmPlatformRepositoriesContainer();
            var user = context.UsersRepository.GetBy(new Query<User>(e => e.Id == id));
            return user.FullName;
        }

        public static string GetStatusName(int id)
        {
            var status = context.BugStatuses.Find(id);
            return status.Name;
        }

        public static string GetSeverityName(int id)
        {
            var severity = context.BugSeverities.Find(id);
            return severity.Name;
        }

        public static string GetSymptomName(int id)
        {
            var symptom = context.BugSymptoms.Find(id);
            return symptom.Name;
        }
    }
}