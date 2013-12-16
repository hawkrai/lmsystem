using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.SubjectViewModels
{
    public class SubjectViewModel
    {
        public SubjectViewModel()
        {
        }

        public SubjectViewModel(Subject model)
        {
            SubjectId = model.Id;
            DisplayName = model.Name;
            Name = model.ShortName;
        }

        public int SubjectId
        {
            get; 
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public string Name
        {
            get; 
            set;
        }
    }
}