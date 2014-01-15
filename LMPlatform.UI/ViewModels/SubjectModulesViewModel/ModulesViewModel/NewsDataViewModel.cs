using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel
{
    public class NewsDataViewModel
    {
        public NewsDataViewModel(SubjectNews news)
        {
            Body = news.Body;
            Title = news.Title;
            DateCreate = news.EditDate.ToShortDateString();
        }

        public string Body
        {
            get;
            set;
        }

        public string Title
        {
            get; 
            set;
        }

        public string DateCreate
        {
            get; 
            set;
        }
    }
}