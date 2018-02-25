using System.ComponentModel;
using System.Web;
using Application.Core.UI.HtmlHelpers;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.UI.ViewModels.KnowledgeTestingViewModels
{
    public class QuestionItemListViewModel : BaseNumberedGridItem
    {
        [DisplayName("Вопрос")]
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

        public bool Selected
        {
            get;
            set;
        }

        public int? QuestionNumber
        {
            get;
            set;
        }

        public static QuestionItemListViewModel FromQuestion(Question question, string htmlString)
        {
            var model = FromQuestion(question);
            model.Action = new HtmlString(htmlString);
            model.QuestionNumber = question.QuestionNumber;
            return model;
        }

        public static QuestionItemListViewModel FromQuestion(Question question)
        {
            return new QuestionItemListViewModel
            {
                Id = question.Id,
                Title = question.Title,
                QuestionNumber = question.QuestionNumber
            };
        }
    }
}