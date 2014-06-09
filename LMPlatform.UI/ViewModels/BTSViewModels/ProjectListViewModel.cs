using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Application.Core.UI.HtmlHelpers;

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

        [DisplayName("Кол-во участников проекта")]
        public int UserQuentity { get; set; }

        [DisplayName("Действие")]
        public HtmlString Action
        {
            get;
            set;
        }

        public int Id { get; set; }

        public bool IsAssigned { get; set; }
    }
}