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

        public int Id
        {
            get;
            set;
        }

        public bool Selected
        {
            get;
            set;
        }

        public static QuestionItemListViewModel FromQuestion(Question question, string htmlString)
        {
            var model = FromQuestion(question);
            model.Action = new HtmlString(htmlString);

            return model;
        }

        public static QuestionItemListViewModel FromQuestion(Question question)
        {
            return new QuestionItemListViewModel
            {
                Id = question.Id,
                Title = question.Title,
                Description = question.Description
            };
        }
    }
}