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

        public int Id
        {
            get; 
            set;
        }

        public static TestItemListViewModel FromTest(Test test, string htmlLinks)
        {
            var model = FromTest(test);
            model.Action = new HtmlString(htmlLinks);

            return model;
        }

        public static TestItemListViewModel FromTest(Test test)
        {
            return new TestItemListViewModel
            {
                Id = test.Id,
                Title = test.Title,
                Description = test.Description
            };
        }
    }
}