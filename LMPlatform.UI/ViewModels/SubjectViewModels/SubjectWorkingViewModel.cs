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
            Modules = Subject.SubjectModules.Select(e => new ModulesViewModel(e.Module)).ToList();
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