using System.ComponentModel;
using System.Web;
using Application.Core.UI.HtmlHelpers;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.UI.ViewModels.KnowledgeTestingViewModels
{
    public class TestItemListViewModel : BaseNumberedGridItem
    {
        [DisplayName("Название")]
        public string Title
        {
            get;
            set;
        }

        [DisplayName("Действия")]
        public HtmlString Action
        {
            get;
            set;
        }

        public int Id
        {
            get; 
            set;
        }

        public int? TestNumber
        {
            get;
            set;
        }

        public bool Unlocked
        {
            get; 
            set;
        }

        public bool HasQuestions
        {
            get; 
            set;
        }

        public bool ForSelfStudy
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
                Unlocked = test.Unlocked,
                HasQuestions = test.Questions == null ? false : test.Questions.Count > 0,
                ForSelfStudy = test.ForSelfStudy,
                TestNumber = test.TestNumber
            };
        }
    }
}