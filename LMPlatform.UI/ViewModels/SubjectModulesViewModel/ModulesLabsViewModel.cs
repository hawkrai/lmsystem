namespace LMPlatform.UI.ViewModels.SubjectModulesViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Application.Core;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.Models;
    using LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel;

    public class ModulesLabsViewModel : ModulesBaseViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
            }
        }

        public IList<LabsDataViewModel> LabsData
        {
            get;
            set;
        }

        public ModulesLabsViewModel(int subjectId, Module module)
            : base(subjectId, module)
        {
            LabsData =
                SubjectManagementService.GetSubject(subjectId).Labs.Select(e => new LabsDataViewModel(e)).ToList();
        }
    }
}