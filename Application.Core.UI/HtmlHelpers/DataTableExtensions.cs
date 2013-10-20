using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using Mvc.JQuery.Datatables;

namespace Application.Core.UI.HtmlHelpers
{
	public static class DataTableExtensions
	{
		#region DataTableExtensions Members

        public static DataTableConfigVm DataTable<TController, TResult>(this HtmlHelper html, string id, Expression<Func<TController, DataTablesResult<TResult>>> exp, object routes = null, IEnumerable<ColDef> columns = null)
		{
			var dataTable = html.DataTableVm(id, exp, columns);

			if (routes != null)
			{
				var routeValues = new RouteValueDictionary(routes);

				string ajaxUrl = routeValues.Aggregate(dataTable.AjaxUrl, (current, routeParameter) => current + string.Format("?{0}={1}", routeParameter.Key, routeParameter.Value));

				dataTable = new DataTableConfigVm(dataTable.Id, ajaxUrl, dataTable.Columns);
			}

			return dataTable;
		}

		#endregion DataTableExtensions Members
	}
}
