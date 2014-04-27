using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using Application.Core.UI.HtmlHelpers;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class ProjectListViewModel : BaseNumberedGridItem
    {
        [DataType(DataType.Text)]
        [DisplayName("Тема проекта")]
        public string Title { get; set; }

        [DisplayName("Дата создания")]
        public string CreationDate { get; set; }

        [DisplayName("Создатель")]
        public string CreatorName { get; set; }

        [DisplayName("Действие")]
        public HtmlString Action
        {
            get;
            set;
        }

        public int Id { get; set; }

        public bool IsAssigned { get; set; }

        public static ProjectListViewModel FromProject(Project project, string htmlLinks)
        {
            var model = FromProject(project);
            model.Action = new HtmlString(htmlLinks);

            return model;
        }

        public static ProjectListViewModel FromProject(Project project)
        {
            var context = new LmPlatformModelsContext().ProjectUsers;
            var isAssigned = false;
            foreach (var user in context)
            {
                if (user.ProjectId == project.Id && user.UserId == WebSecurity.CurrentUserId)
                {
                    isAssigned = true;
                }
            }
            
            return new ProjectListViewModel
                {
                    Id = project.Id,
                    Title = project.Title,
                    CreatorName = project.Creator.UserName,
                    CreationDate = project.CreationDate.ToShortDateString(),
                    IsAssigned = isAssigned
                };
        }
    }
}