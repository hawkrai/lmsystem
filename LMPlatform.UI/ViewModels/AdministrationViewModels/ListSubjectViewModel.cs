using System.Collections.Generic;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
	using Application.Core.UI.HtmlHelpers;

	public class ListSubjectViewModel : BaseNumberedGridItem
	{
		public List<Subject> Subjects { get; set; }

		public string Name { get; set; }

		public static ListSubjectViewModel FormSubjects(List<Subject> subjects, string name)
		{
			return new ListSubjectViewModel
			{
				Subjects = subjects,
				Name = name
			};
		}
	}
}