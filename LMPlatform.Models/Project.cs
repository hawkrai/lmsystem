using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class Project : ModelBase
    {
        [Display(Name = "Тема проекта")]
        public string Title
        {
            get; 
            set;
        }

        [Display(Name = "Дата создания")]
        public DateTime CreationDate
        {
            get; 
            set;
        }

        [Display(Name = "Создатель")]
        public int CreatorId
        {
            get; 
            set; 
        }

        [Display(Name = "Избранный")]
        public int IsChosen
        {
            get; 
            set;
        }

        public User Creator
        { 
            get; 
            set; 
        }

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
