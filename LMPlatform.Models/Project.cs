using System;
using System.Collections.Generic;
using System.ComponentModel;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class Project : ModelBase
    {
        [DisplayName("Тема проекта")]
        public string Title { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Создатель")]
        public int CreatorId { get; set; }

        [DisplayName("Избранный")]
        public bool IsChosen { get; set; }

        //[DisplayName("Создатель")]
        public User Creator { get; set; }

        public ICollection<ProjectStudent> ProjectStudents
        {
            get; 
            set; 
        }

        public ICollection<Bug> Bugs
        {
            get;
            set;
        }
    }
}
