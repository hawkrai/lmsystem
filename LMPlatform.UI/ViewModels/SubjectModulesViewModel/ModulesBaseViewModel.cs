using Application.Core;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.SubjectModulesViewModel
{
    public class ModulesBaseViewModel
    {
        public int SubjectId
        {
            get;
            set;
        }

        public Module Module
        {
            get;
            set;
        }

        public ModulesBaseViewModel(int subjectId, Module module)
        {
            SubjectId = subjectId;
            Module = module;
        }
    }
}