using System.ComponentModel;
using System.Web;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.UI.ViewModels.KnowledgeTestingViewModels
{
    public class TestItemListViewModel
    {
        [DisplayName("")]
        public HtmlString Action
        {
            get;
            set;
        }

        [DisplayName("Название")]
        public string Title
        {
            get;
            set;
        }

        [DisplayName("Описание")]
        public string Description
        {
            get;
            set;
        }

        public static TestItemListViewModel FromTest(Test test, string htmlLinks)
        {
            return new TestItemListViewModel
            {
                Title = test.Title,
                Description = test.Description,
                Action = new HtmlString(htmlLinks)
            };
        }
    }
}