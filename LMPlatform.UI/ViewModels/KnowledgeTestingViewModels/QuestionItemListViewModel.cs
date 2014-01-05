using System.ComponentModel;
using System.Web;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.UI.ViewModels.KnowledgeTestingViewModels
{
    public class QuestionItemListViewModel
    {
        [DisplayName("")]
        public HtmlString Action
        {
            get;
            set;
        }

        [DisplayName("Вопрос")]
        public string Title
        {
            get;
            set;
        }

        [DisplayName("Задание")]
        public string Description
        {
            get;
            set;
        }

        public static QuestionItemListViewModel FromQuestion(Question question, string htmlString)
        {
            return new QuestionItemListViewModel
            {
                Title = question.Title,
                Description = question.Description,
                Action = new HtmlString(htmlString)
            };
        }
    }
}