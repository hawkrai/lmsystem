using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.SubjectViewModels
{
    public class SubjectWorkingViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IModulesManagementService> _modulesManagementService = new LazyDependency<IModulesManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

        public IModulesManagementService ModulesManagementService
        {
            get
            {
                return _modulesManagementService.Value;
            }
        }

        public IList<ModulesViewModel> Modules
        {
            get;
            set;
        }

        public IList<ModulesViewModel> NotVisibleModules
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

	    public Subject Subject
	    {
		    get;
		    set;
	    }

        public SubjectWorkingViewModel(int subjectId)
        {
            SubjectId = subjectId;
            Subject = SubjectManagementService.GetSubject(subjectId);
            SubjectName = Subject.Name;
            Modules = Subject.SubjectModules.OrderBy(e => e.Module.Order).Select(e => new ModulesViewModel(e.Module)).ToList();
            NotVisibleModules = ModulesManagementService.GetModules().Where(e => !e.Visible).Select(e => new ModulesViewModel(e)).ToList();
        }

        public SubGroupEditingViewModel SubGroup(int groupId)
        {
            return new SubGroupEditingViewModel(SubjectId, groupId);
        }

        public SubGroupEditingViewModel SubGroups
	    {
		    get
		    {
			    return new SubGroupEditingViewModel(SubjectId);
		    }
	    }
    }
}