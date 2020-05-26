using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using Microsoft.Ajax.Utilities;

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

		public List<SelectListItem> SubGroupsThirdList
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
			SubGroupsThirdList = new List<SelectListItem>();

			SubGroupsFirstList = subGroups.FirstOrDefault(e => e.Name == "first").SubjectStudents.Where(e => e.Student.Confirmed == null || e.Student.Confirmed.Value).Select(e => new SelectListItem
			{
				Text = e.Student.FullName,
				Value = e.Student.Id.ToString(CultureInfo.InvariantCulture),
				Selected = false
			}).ToList();

			SubGroupsTwoList = subGroups.FirstOrDefault(e => e.Name == "second").SubjectStudents.Where(e => e.Student.Confirmed == null || e.Student.Confirmed.Value).Select(e => new SelectListItem
			{
                Text = e.Student.FullName,
				Value = e.Student.Id.ToString(CultureInfo.InvariantCulture),
				Selected = false
			}).ToList();

			SubGroupsThirdList = subGroups.FirstOrDefault(e => e.Name == "third").SubjectStudents.Where(e => e.Student.Confirmed == null || e.Student.Confirmed.Value).Select(e => new SelectListItem
			{
				Text = e.Student.FullName,
				Value = e.Student.Id.ToString(CultureInfo.InvariantCulture),
				Selected = false
			}).ToList();
		}

	    public SubGroupEditingViewModel()
	    {
	    }

		public void SaveSubGroups(int subjectId, int groupId, string subGroupFirst, string subGroupSecond, string subGroupThird)
	    {
            var listSubGroupFirst = new List<int>();
            var listSubGroupSecond = new List<int>();
			var listSubGroupThird = new List<int>();

	        if (!subGroupFirst.IsNullOrWhiteSpace())
	        {
	            subGroupFirst = subGroupFirst.Remove(subGroupFirst.Length - 1);    
                listSubGroupFirst = subGroupFirst.Split(new[] { ',' }).Select(int.Parse).ToList();
	        }

	        if (!subGroupSecond.IsNullOrWhiteSpace())
	        {
	            subGroupSecond = subGroupSecond.Remove(subGroupSecond.Length - 1);
                listSubGroupSecond = subGroupSecond.Split(new[] { ',' }).Select(int.Parse).ToList();
	        }

			if (!subGroupThird.IsNullOrWhiteSpace())
			{
				subGroupThird = subGroupThird.Remove(subGroupThird.Length - 1);
				listSubGroupThird = subGroupThird.Split(new[] { ',' }).Select(int.Parse).ToList();
			}

			SubjectManagementService.SaveSubGroup(subjectId, groupId, listSubGroupFirst, listSubGroupSecond, listSubGroupThird);
	    }

		public SubGroupEditingViewModel(int subjectId, int groupId = 0)
		{
			var groups = GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == subjectId && x.IsActiveOnCurrentGroup)).Include(e => e.Students));
			FillGroupsList(groups);

            if (groupId == 0)
            {
                groupId = int.Parse(GroupsList.FirstOrDefault().Value);
            }

		    GroupId = groupId.ToString(CultureInfo.InvariantCulture);

			var subGroups = SubjectManagementService.GetSubGroupsV3(subjectId, groupId);

			if (subGroups.Any())
			{
				FillSubGroupsList(subGroups);
                StudentGroupList = new List<SelectListItem>();
			    
                foreach (var student in groups.FirstOrDefault(e => e.Id == groupId).Students)
                {
	                if (student.Confirmed == null || student.Confirmed.Value)
	                {
		                var studentId = student.Id.ToString(CultureInfo.InvariantCulture);

					

						if (!SubGroupsFirstList.Any() && !SubGroupsTwoList.Any() && !SubGroupsThirdList.Any())
						{
							StudentGroupList.Add(new SelectListItem
							{
								Selected = false,
								Text = student.FullName,
								Value = studentId
							});
						}
						else
						{
							if (SubGroupsFirstList.Any(e => e.Value == studentId) ||
								SubGroupsTwoList.Any(e => e.Value == studentId) ||
								SubGroupsThirdList.Any(e => e.Value == studentId))
							{
							}
							else
							{
								StudentGroupList.Add(new SelectListItem
								{
									Selected = false,
									Text = student.FullName,
									Value = studentId
								}); 
							}
						}
					}
                }
			}
			else
			{
                SubGroupsFirstList = new List<SelectListItem>();
                SubGroupsTwoList = new List<SelectListItem>();
				SubGroupsThirdList = new List<SelectListItem>();

                StudentGroupList = groups.FirstOrDefault(e => e.Id == groupId).Students.Where(e => e.Confirmed == null || e.Confirmed.Value).Select(e => new SelectListItem
				{
					Text = e.FullName,
					Value = e.Id.ToString(CultureInfo.InvariantCulture),
					Selected = false
				}).ToList();
			}
		}
	}
}