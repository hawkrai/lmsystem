using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel;

namespace LMPlatform.UI.ViewModels.SubjectModulesViewModel
{
    public class ModulesNewsViewModel : ModulesBaseViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

        public List<NewsDataViewModel> News
        {
            get;
            set;
        }

        public ModulesNewsViewModel(int subjectId, Module module) : base(subjectId, module)
        {
            News = SubjectManagementService.GetSubject(subjectId).SubjectNewses.Select(e => new NewsDataViewModel(e)).ToList();
        }
    }
}