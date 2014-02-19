using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel;

namespace LMPlatform.UI.ViewModels.SubjectModulesViewModel
{
	public class ModulesLecturesViewModel : ModulesBaseViewModel
	{
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

	    public IList<LecturesDataViewModel> LecturesData
	    {
	        get;
	        set;
	    }

		public ModulesLecturesViewModel(int subjectId, Module module) : base(subjectId, module)
		{
		    LecturesData =
		        SubjectManagementService.GetSubject(subjectId).Lectures.Select(e => new LecturesDataViewModel(e)).ToList();
		}
	}
}