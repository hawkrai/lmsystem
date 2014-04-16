namespace LMPlatform.UI.ViewModels.SubjectModulesViewModel
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using Application.Core;
    using Application.Core.Data;
    using Application.Infrastructure.GroupManagement;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.Models;
    using LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel;

    public class ModulesScheduleProtectionLabsViewModel : ModulesBaseViewModel
    {
        private readonly LazyDependency<IGroupManagementService> _groupManagementService = new LazyDependency<IGroupManagementService>();
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

        public IGroupManagementService GroupManagementService
        {
            get
            {
                return _groupManagementService.Value;
            }
        } 

        public IList<ScheduleProtectionLabsDataViewModel> ScheduleProtectionLabs
        {
            get;
            set;
        }

        public string GroupId
        {
            get;
            set;
        }

        public string SubGroupId { get; set; }

        public List<SelectListItem> GroupsList
        {
            get;
            set;
        }

        private void FillGroupsList(IEnumerable<Group> groups)
        {
            GroupsList = new List<SelectListItem>();
            GroupsList = groups.Select(e => new SelectListItem
            {
                Selected = false,
                Value = e.Id.ToString(CultureInfo.InvariantCulture),
                Text = e.Name
            }).ToList();

            if (GroupsList.Any() && GroupsList != null)
            {
                GroupId = GroupsList.First().Value;
            }
        }

        public ModulesScheduleProtectionLabsViewModel(int subjectId, Module module)
            : base(subjectId, module)
        {
            var subject = this.SubjectManagementService.GetSubject(subjectId);
            var firstOrDefault = subject.SubjectGroups.FirstOrDefault();
            if (firstOrDefault != null)
            {
                var defaultOr = firstOrDefault.SubGroups.FirstOrDefault();
                if (defaultOr != null)
                {
                    this.SubGroupId = defaultOr.Id.ToString(CultureInfo.InvariantCulture);
                }
            }

            var subjectGroup = subject.SubjectGroups.FirstOrDefault();
            if (subjectGroup != null)
            {
                this.GroupId = subjectGroup.GroupId.ToString(CultureInfo.InvariantCulture);
            }

            this.ScheduleProtectionLabs = subject.Labs.Select(e => new ScheduleProtectionLabsDataViewModel(e, int.Parse(SubGroupId))).ToList();

            var groups = GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == subjectId)).Include(e => e.Students));
            FillGroupsList(groups);
        }
    }
}