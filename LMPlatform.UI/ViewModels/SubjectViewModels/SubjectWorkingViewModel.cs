using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Infrastructure.SubjectManagement;

namespace LMPlatform.UI.ViewModels.SubjectViewModels
{
    public class SubjectWorkingViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

        public IList<ModulesViewModel> Modules
        {
            get;
            set;
        }

        public int SubjectId
        {
            get;
            set;
        }

        public string SubjectName
        {
            get;
            set;
        }

        public SubjectWorkingViewModel(int subjectId)
        {
            SubjectId = subjectId;
            var subject = SubjectManagementService.GetSubject(subjectId);
            SubjectName = subject.Name;
            Modules = subject.SubjectModules.Select(e => new ModulesViewModel(e.Module)).ToList();
        }
    }
}