namespace LMPlatform.UI.ViewModels.SubjectModulesViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Application.Core;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.Models;
    using LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel;

    public class ModulesPracticalViewModel : ModulesBaseViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
            }
        }

        public IList<PracticalsDataViewModel> PracticalsData
        {
            get;
            set;
        }

        public ModulesPracticalViewModel(int subjectId, Module module)
            : base(subjectId, module)
        {
            PracticalsData =
                SubjectManagementService.GetSubject(subjectId).Practicals.Select(e => new PracticalsDataViewModel(e)).ToList();
        }
    }
}