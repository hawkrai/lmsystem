using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.UI.ViewModels.SubjectViewModels;

namespace LMPlatform.UI.ViewModels.LmsViewModels
{
    public class LmsViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

        public List<SubjectViewModel> Subjects
        {
            get; 
            set;
        } 

        public LmsViewModel(int userId)
        {
            Subjects = SubjectManagementService.GetUserSubjects(userId).Where(e => !e.IsArchive).Select(e => new SubjectViewModel(e)).ToList();
        }
    }
}