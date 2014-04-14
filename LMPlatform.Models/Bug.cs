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
        public User Creator { get; set; }

        [Display(Name = "Название")]
        public string Summary { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Шаги выполнения")]
        public string Steps { get; set; }

        [Display(Name = "Дата документирования")]
        public DateTime CreatingDate { get; set; }

        [Display(Name = "Дата последнего изменения")]
        public DateTime ModifyingDate { get; set; }

        [Display(Name = "Симптом")]
        public int SymptomId { get; set; }

        [Display(Name = "Важность")]
        public int SeverityId { get; set; }

        [Display(Name = "Статус")]
        public int StatusId { get; set; }

        public int CreatorId { get; set; }

        public Project Project { get; set; }

        public BugSymptom Symptom { get; set; }

        public BugSeverity Severity { get; set; }

        public BugStatus Status { get; set; }
    }
}
