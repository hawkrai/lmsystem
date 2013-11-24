using Mvc.JQuery.Datatables;

namespace Application.Core.UI.HtmlHelpers
{
	public class DataTableViewModel
	{
		public DataTableVm DataTable
		{
			get;
			set;
		}

		public DataTableOptions DataTableOptions
		{
			get;
			set;
		}

		public DataTableViewModel()
		{
			DataTableOptions = new DataTableOptions();
		}
	}
}