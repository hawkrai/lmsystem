using System;
using System.ComponentModel.DataAnnotations;
using Application.Core.Data;
using LMPlatform.Models.BTS;

namespace LMPlatform.Models
{
    public class Bug : ModelBase
    {
        public int ProjectId { get; set; }

        [Display(Name = "Кем добавлена")]
        public int StudentId { get; set; }

        [Display(Name = "Содержание")]
        public string Summary { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Шаги выполнения")]
        public string Steps { get; set; }

        [Display(Name = "Дата документирования")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Дата изменения")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Симптом")]
        public int SymptomId { get; set; }

        [Display(Name = "Важность")]
        public int SeverityId { get; set; }

        [Display(Name = "Статус")]
        public int StatusId { get; set; }

        public Student Student { get; set; }

        public Project Project { get; set; }

        public BugSymptom Symptom { get; set; }

        public BugSeverity Severity { get; set; }

        public BugStatus Status { get; set; }
    }
}
