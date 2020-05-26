using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using Application.Core.Data;
using Application.Core.Extensions;
using Mvc.JQuery.Datatables;

namespace Application.Core.UI.HtmlHelpers
{
    public static class DataTableExtensions
    {
        #region DataTableExtensions Members

        public static DataTableVm DataTable<TController, TResult>(this HtmlHelper html, string id, Expression<Func<TController, DataTablesResult<TResult>>> exp, object routes = null, IEnumerable<ColDef> columns = null)
        {
            var visibleColumns = ExtractColumnNames<TResult>();
            var dataTable = html.DataTableVm(id, exp, visibleColumns);

            if (routes != null)
            {
                var routeValues = new RouteValueDictionary(routes);

                string ajaxUrl = routeValues.Aggregate(dataTable.AjaxUrl, (current, routeParameter) => current + string.Format("?{0}={1}", routeParameter.Key, routeParameter.Value));

                dataTable = new DataTableVm(dataTable.Id, ajaxUrl, visibleColumns);
            }

            return dataTable;
        }

        public static IEnumerable<ISortCriteria> GetSortCriterias<TModel>(this DataTablesParam param)
        {
            var columns = (from p in TypeExtensions.GetSortedProperties<TModel>()
                           select new ColInfo(p.Name, p.PropertyType)).ToArray<ColInfo>();
            var result = new List<ISortCriteria>(); 

            for (int k = 0; k < param.iSortingCols; k++)
            {
                int num = param.iSortCol[k];
                string name = columns[num].Name;
                string direction = param.sSortDir[k];

                result.Add(new SortCriteria(name, direction.ToUpperFirst()));
            }

            return result;
        }

        public static IPageInfo ToPageInfo(this DataTablesParam param)
        {
            return new PageInfo
            {
				PageNumber = (param.iDisplayStart / param.iDisplayLength) + 1,
				PageSize = param.iDisplayLength == -1 ? 0 : param.iDisplayLength
            };
        }

        public static string GetSearchString(this DataTablesParam param)
        {
            var result = param.sSearch;
            param.sSearch = string.Empty;

            return result;
        }

        private static ColDef[] ExtractColumnNames<TModel>()
        {
            var sortedProperties = TypeExtensions.GetSortedProperties<TModel>();
            var columns = new List<ColDef>();
            foreach (var current in sortedProperties)
            {
                var displayNameAttribute = (DisplayNameAttribute)current.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();

                if (displayNameAttribute != null)
                {
                    var displayName = displayNameAttribute.DisplayName;
                    columns.Add(new ColDef
                    {
                        Name = current.Name,
                        DisplayName = displayName,
                        Type = current.PropertyType
                    });
                }
            }

            return columns.ToArray();
        }

        public static DataTablesResult<T> GetResults<T>(IEnumerable<T> data, DataTablesParam param, int allItemCount)
        {
            data = data.ToList();

            if (typeof(T).BaseType == typeof(BaseNumberedGridItem))
            {
                RenumberItems(data.Cast<BaseNumberedGridItem>(), param);
            }

            var searchCriteries = param.GetSortCriterias<T>();

            data = searchCriteries.Aggregate(data, (current, searchCritery) => searchCritery.SortDirection == SortDirection.Asc ? current.OrderByAsc(searchCritery.Name) : current.OrderByDesc(searchCritery.Name));

            var properties = TypeExtensions.GetSortedProperties<T>();
            var source = data.Select(i => new
            {
                i,
                pairs = from p in properties
                        select new
                        {
                            p.PropertyType,
                            Value = p.GetGetMethod().Invoke(i, null)
                        }
            }).Select(@t => new
            {
                @t,
                values = from p in @t.pairs select GetNormalizedValue(p.Value)
            }).Select(@t => @t.values);
 
            return new DataTablesResult<T>
            {
                Data = new DataTablesData
                {
                    iTotalRecords = allItemCount,
                    iTotalDisplayRecords = allItemCount,
                    sEcho = param.sEcho,
                    aaData = source.ToArray()
                },
                MaxJsonLength = Int32.MaxValue
            };
        }

        private static object GetNormalizedValue(object value)
        {
            return value != null && !(value is string) ? value.ToString() : value;
        }

        private static void RenumberItems<T>(IEnumerable<T> data, DataTablesParam param) where T : BaseNumberedGridItem
        {
            int itemNumber = param.iDisplayStart + 1;
            foreach (T item in data)
            {
                item.Number = itemNumber++;
            }
        }

        #endregion DataTableExtensions Members
    }
}
