using Application.Core.Helpers;
using Mvc.JQuery.Datatables;

namespace Application.Core.UI.HtmlHelpers
{
	public class DataTableViewModel
	{
        public DataTableConfigVm DataTable
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

		public string JsonSerialize(object serializingObject)
		{
			return JsonHelper.SerializeObject(serializingObject);
		}
	}
}