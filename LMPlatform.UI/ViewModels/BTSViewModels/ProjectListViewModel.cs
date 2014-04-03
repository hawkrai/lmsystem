using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
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
        public string CreationDate { get; set; }

        [DisplayName("Создатель")]
        public string CreatorName { get; set; }

        public static ProjectListViewModel FromProject(Project project)
        {
            return new ProjectListViewModel
                {
                    Title = project.Title,
                    CreatorName = project.Creator.UserName,
                    CreationDate = project.CreationDate.ToShortDateString()
                };
        }
    }
}