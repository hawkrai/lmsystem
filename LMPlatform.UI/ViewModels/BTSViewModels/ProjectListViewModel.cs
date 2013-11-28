using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class ProjectListViewModel
    {
        [Display(Name = "ID проекта")]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Тема проекта")]
        public string Title { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Создатель")]
        public User Creator { get; set; }

        [Display(Name = "Избранный")]
        public int IsChosen { get; set; }

        public static ProjectListViewModel FromProject(Project project)
        {
            return new ProjectListViewModel
                {
                    Id = project.Id,
                    Title = project.Title,
                    Creator = project.Creator,
                    CreationDate = project.CreationDate,
                    IsChosen = project.IsChosen
                };
        }
    }
}