using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.SubjectViewModels
{
	public class SubGroupEditingViewModel
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

		public string GroupId
		{
			get;
			set;
		}

		public List<SelectListItem> GroupsList
		{
			get;
			set;
		}

		public List<SelectListItem> StudentGroupList
		{
			get;
			set;
		}

		public List<SelectListItem> SubGroupsFirstList
		{
			get;
			set;
		}

		public List<SelectListItem> SubGroupsTwoList
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

		private void FillSubGroupsList(IEnumerable<SubGroup> subGroups)
		{
			SubGroupsFirstList = new List<SelectListItem>();
			SubGroupsTwoList = new List<SelectListItem>();

			SubGroupsFirstList = subGroups.FirstOrDefault().SubjectStudents.Select(e => new SelectListItem
			{
				Text = e.Student.FirstName,
				Value = e.Id.ToString(CultureInfo.InvariantCulture),
				Selected = false
			}).ToList();

			SubGroupsTwoList = subGroups.LastOrDefault().SubjectStudents.Select(e => new SelectListItem
			{
				Text = e.Student.FirstName,
				Value = e.Id.ToString(CultureInfo.InvariantCulture),
				Selected = false
			}).ToList();
		}

		public SubGroupEditingViewModel(int subjectId)
		{
			var groups = GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == subjectId)).Include(e => e.Students));
			FillGroupsList(groups);

			var subGroups = SubjectManagementService.GetSubGroups(subjectId, int.Parse(GroupsList.FirstOrDefault().Value));
			if (subGroups.Any())
			{
				FillSubGroupsList(subGroups);	
			}
			else
			{
				StudentGroupList = groups.FirstOrDefault(e => e.Id == int.Parse(GroupsList.FirstOrDefault().Value)).Students.Select(e => new SelectListItem
				{
					Text = e.FullName,
					Value = e.Id.ToString(CultureInfo.InvariantCulture),
					Selected = false
				}).ToList();
			}
		}
	}
}