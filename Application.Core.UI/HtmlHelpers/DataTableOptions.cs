using System.Collections.Generic;

namespace Application.Core.UI.HtmlHelpers
{
	public class DataTableOptions
	{
		public bool Pagination
		{
			get;
			set;
		}

		public bool Searchable
		{
			get;
			set;
		}

		public bool Sortable
		{
			get;
			set;
		}

		public bool Filterable
		{
			get;
			set;
		}

		public bool Info
		{
			get;
			set;
		}

		public string OnComplete
		{
			get;
			set;
		}

		public DataTableOptions()
		{
			Pagination = true;
			Searchable = true;
			Sortable = true;
			Filterable = true;
			Info = true;
		}
	}
}
