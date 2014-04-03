using System;
using System.ComponentModel;
using Application.Core.Data;
using Application.Infrastructure.BugManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using LMPlatform.Models.BTS;
using Microsoft.Ajax.Utilities;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class BugListViewModel
    { 
        [DisplayName("Проект")]
        public string Project { get; set; }

        [DisplayName("Содержание")]
        public string Summary { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        [DisplayName("Шаги выполнения")]
        public string Steps { get; set; }

        [DisplayName("Симптом")]
        public string Symptom { get; set; }

        [DisplayName("Важность")]
        public string Severity { get; set; }

        [DisplayName("Статус")]
        public string Status { get; set; }

        [DisplayName("Кем добавлена")]
        public string ReporterName { get; set; }

        [DisplayName("Дата документирования")]
        public string ReportingDate { get; set; }

        [DisplayName("Дата последнего изменения")]
        public string ModifyingDate { get; set; }

        private static LmPlatformModelsContext context = new LmPlatformModelsContext();

        public static BugListViewModel FromBug(Bug bug)
        {
            return new BugListViewModel
            {
                Summary = bug.Summary,
                Description = bug.Description,
                Steps = bug.Steps,
                Symptom = GetSymptomName(bug.SymptomId),
                Severity = GetSeverityName(bug.SeverityId),
                Status = GetStatusName(bug.StatusId),
                Project = GetProjectTitle(bug.ProjectId),
                ReporterName = GetCreatorName(bug.CreatorId),
                ReportingDate = bug.CreatingDate.ToShortDateString(),
                ModifyingDate = bug.ModifyingDate.ToShortDateString()
            };
        }

        public static string GetProjectTitle(int id)
        {
            var projectManagementService = new ProjectManagementService();
            var project = projectManagementService.GetProject(id);
            return project.Title;
        }

        public static string GetCreatorName(int id)
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