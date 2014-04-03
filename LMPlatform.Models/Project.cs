using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class Project : ModelBase
    {
        [Required]
        [DisplayName("Тема проекта")]
        public string Title { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Создатель")]
        public int CreatorId { get; set; }

        //[DisplayName("Создатель")]
        public User Creator { get; set; }

        public ICollection<ProjectUser> ProjectUsers
        {
            get; 
            set; 
        }

        public ICollection<Bug> Bugs
        {
            get;
            set;
        }

        public ICollection<ProjectRole> ProjectRoles
        {
            get; set; 
        }
    }
}
