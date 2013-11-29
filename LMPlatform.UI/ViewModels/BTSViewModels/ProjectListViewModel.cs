using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class ProjectListViewModel
    {
        [DataType(DataType.Text)]
        [DisplayName("Тема проекта")]
        public string Title { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Создатель")]
        public string Creator { get; set; }

        [DisplayName("Избранный")]
        public bool IsChosen { get; set; }

        public static ProjectListViewModel FromProject(Project project)
        {
            return new ProjectListViewModel
                {
                    Title = project.Title,
                    Creator = project.Creator.UserName,
                    CreationDate = project.CreationDate,
                    IsChosen = project.IsChosen
                };
        }
    }
}