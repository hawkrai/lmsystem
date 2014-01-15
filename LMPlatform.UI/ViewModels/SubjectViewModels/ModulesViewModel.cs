using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.SubjectViewModels
{
    public class ModulesViewModel
    {
        public ModulesViewModel(Module module, bool check = false)
        {
            Name = module.DisplayName;
            ModuleId = (int)module.ModuleType;
            Checked = check;
        }

        public ModulesViewModel()
        {
        }

        public string Name
        {
            get;
            set;
        }

        public int ModuleId
        {
            get; 
            set;
        }

        public bool Checked
        {
            get;
            set;
        }
    }
}