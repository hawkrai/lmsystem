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

        [DisplayName("Кем добавлена")]
        public string ReporterName { get; set; }

        [DisplayName("Название")]
        public string Summary { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        [DisplayName("Шаги выполнения")]
        public string Steps { get; set; }

        [DisplayName("Ожидаемый результат")]
        public string ExpectedResult { get; set; }

        [DisplayName("Важность")]
        public string Severity { get; set; }

        [DisplayName("Статус")]
        public string Status { get; set; }

        [DisplayName("Симптом")]
        public string Symptom { get; set; }

        [DisplayName("Проект")]
        public string Project { get; set; }

        [DisplayName("Дата документирования")]
        public string ReportingDate { get; set; }

        [DisplayName("Дата последнего изменения")]
        public string ModifyingDate { get; set; }

        [DisplayName("Кем изменена")]
        public string EditorName { get; set; }

        [DisplayName("Назначенный разработчик")]
        public string AssignedDeveloperName { get; set; }

        public int SymptomId { get; set; }

        public int SeverityId { get; set; }

        public int StatusId { get; set; }

        public int ReporterId { get; set; }

        public int EditorId { get; set; }

        public int AssignedDeveloperId { get; set; }

        public int BugId { get; set; }

        public BugsViewModel(int id)
        {
            var model = new BugManagementService().GetBug(id);
            BugId = id;
            SetParams(model);
        }

        public void SetParams(Bug model)
        {
            var context = new ProjectManagementService();

            Steps = model.Steps;
            ExpectedResult = model.ExpectedResult;
            Symptom = GetSymptomName(model.SymptomId);
            EditorName = context.GetCreatorName(model.EditorId);
            ReporterName = context.GetCreatorName(model.ReporterId);
            Summary = model.Summary;
            Description = model.Description;
            Severity = GetSeverityName(model.SeverityId);
            Status = GetStatusName(model.StatusId);
            Project = GetProjectTitle(model.ProjectId);
            ProjectId = model.ProjectId;
            ModifyingDate = model.ModifyingDate.ToShortDateString();
            ReportingDate = model.ReportingDate.ToShortDateString();
            AssignedDeveloperId = model.AssignedDeveloperId;
            AssignedDeveloperName = (AssignedDeveloperId == 0) ? "-" : context.GetCreatorName(AssignedDeveloperId);
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

        public string GetStatusName(int id)
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

        public List<BugLog> GetBugLogs()
        {
            var bugLogList = new BugManagementService().GetBugLogs(BugId).ToList();

            return bugLogList;
        }
    }
}