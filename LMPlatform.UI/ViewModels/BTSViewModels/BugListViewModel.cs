using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        [DisplayName("ID")]
        public int Id { get; set; }
        
        [DisplayName("Название")]
        public string Summary { get; set; }

        [DisplayName("Важность")]
        public string Severity { get; set; }

        [DisplayName("Статус")]
        public string Status { get; set; }

        [DisplayName("Назначенный разработчик")]
        public string AssignedDeveloperName { get; set; }

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

        public string AssignedDeveloperId { get; set; }

        public int ProjectId { get; set; }

        public int StatusId { get; set; }

        public int CurrentProjectId { get; set; }

        public string CurrentProjectName { get; set; }

        public bool IsAssigned { get; set; }

        public BugListViewModel()
        {
        }

        public BugListViewModel(int id)
        {
            CurrentProjectId = id;
            CurrentProjectName = string.Empty;
            if (id != 0)
            {
                var project = new ProjectManagementService().GetProject(id);
                CurrentProjectName = project.Title;
            }
        }
    }
}