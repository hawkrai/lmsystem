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
        [DisplayName("Избранный")]
        public bool IsChosen { get; set; }
        
        [DataType(DataType.Text)]
        [DisplayName("Тема проекта")]
        public string Title { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Создатель")]
        public string CreatorName { get; set; }

        [DisplayName("Создатель")]
        public User Creator { get; set; }

        public static ProjectListViewModel FromProject(Project project)
        {
            //Image image = Image.FromFile(project.IsChosen ? "/LMPlatform.UI/Content/images/star_blue.png" : "/LMPlatform.UI/Content/images/star_white.png");
            return new ProjectListViewModel
                {
                    Title = project.Title,
                    CreatorName = project.Creator.UserName,
                    CreationDate = project.CreationDate.Date,
                    IsChosen = project.IsChosen
                };
        }
    }
}