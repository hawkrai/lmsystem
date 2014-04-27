using System;
using System.ComponentModel;
using System.Web;
using Application.Core;
using Application.Core.Data;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.BugManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using LMPlatform.Models.BTS;
using Microsoft.Ajax.Utilities;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class BugListViewModel : BaseNumberedGridItem
    {
        private static LmPlatformModelsContext context = new LmPlatformModelsContext();
        
        [DisplayName("Название")]
        public string Summary { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        [DisplayName("Важность")]
        public string Severity { get; set; }

        [DisplayName("Статус")]
        public string Status { get; set; }

        [DisplayName("Проект")]
        public string Project { get; set; }

        [DisplayName("Дата последнего изменения")]
        public string ModifyingDate { get; set; }

        [DisplayName("Действие")]
        public HtmlString Action { get; set; }

        public string Steps { get; set; }

        public string Symptom { get; set; }

        public string ReporterName { get; set; }

        public string ReportingDate { get; set; }

        public int Id { get; set; }

        public static BugListViewModel FromBug(Bug bug, string htmlLinks)
        {
            var model = FromBug(bug);
            model.Action = new HtmlString(htmlLinks);

            return model;
        }

        public static BugListViewModel FromBug(Bug bug)
        {
            return new BugListViewModel
            {
                Id = bug.Id,
                Steps = bug.Steps,
                Symptom = GetSymptomName(bug.SymptomId),
                ReporterName = GetReporterName(bug.ReporterId),
                ReportingDate = bug.ReportingDate.ToShortDateString(),
                Summary = bug.Summary,
                Description = bug.Description,
                Severity = GetSeverityName(bug.SeverityId),
                Status = GetStatusName(bug.StatusId),
                Project = GetProjectTitle(bug.ProjectId),
                ModifyingDate = bug.ModifyingDate.ToShortDateString()
            };
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