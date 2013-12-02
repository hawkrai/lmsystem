using System;
using System.ComponentModel;
using LMPlatform.Models;
using LMPlatform.Models.BTS;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class BugListViewModel
    {
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
        public string Student { get; set; }

        [DisplayName("Дата документирования")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Дата изменения")]
        public DateTime ModifiedDate { get; set; }

        [DisplayName("Проект")]
        public string Project { get; set; }

        public static BugListViewModel FromBug(Bug bug)
        {
            return new BugListViewModel
            {
                Summary = bug.Summary,
                Description = bug.Description,
                Steps = bug.Steps,
                Symptom = bug.Symptom.Name,
                Severity = bug.Severity.Name,
                Status = bug.Status.Name,
                Project = bug.Project.Title,
                Student = bug.Student.FirstName,
                CreationDate = bug.CreationDate,
                ModifiedDate = bug.ModifiedDate
            };
        }
    }
}