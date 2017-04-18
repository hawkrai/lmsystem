using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Application.Core.UI.HtmlHelpers;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    //TODO: Remove inheritance
    public class ProjectListViewModel : BaseNumberedGridItem
    {
        [DataType(DataType.Text)]
        [DisplayName("Тема проекта")]
        public string Title { get; set; }

        [DisplayName("Дата создания")]
        public string CreationDate { get; set; }

        [DisplayName("Создатель")]
        public string CreatorName { get; set; }

        [DisplayName("Кол-во участников проекта")]
        public int UserQuentity { get; set; }

        //TODO: Remove field
        [DisplayName("Действие")]
        public HtmlString Action
        {
            get;
            set;
        }

        public int Id { get; set; }

        //TODO: Remove field
        public bool IsAssigned { get; set; }

        public ProjectListViewModel()
        {
        }

        public ProjectListViewModel(Project project)
        {
            Id = project.Id;
            Title = project.Title;
            CreatorName = project.Creator.FullName;
            CreationDate = project.DateOfChange.ToShortDateString();
            UserQuentity = project.ProjectUsers.Count;
        }
    }
}