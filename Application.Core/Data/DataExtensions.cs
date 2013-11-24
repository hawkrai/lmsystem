using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Application.Core.Data
{
	public static class DataExtensions
	{
		public static IOrderedQueryable<T> OrderByAsc<T>(this IQueryable<T> source, string propertyName)
		{
			var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
			Expression<Func<T, object>> expr = e => propertyInfo.GetValue(e, null);
			return source.OrderBy(expr);
		}

		public static IOrderedQueryable<T> OrderByDesc<T>(this IQueryable<T> source, string propertyName)
		{
			var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
			Expression<Func<T, object>> expr = e => propertyInfo.GetValue(e, null);
			return source.OrderByDescending(expr);
		}

		public static IPageableList<TSource> ToPageableList<TSource>(this IOrderedQueryable<TSource> source, int pageNumber = 1, int pageSize = 0)
		{
			var allItemsCount = source.Count();

			if (pageSize == 0)
			{
				pageSize = allItemsCount;
			}

			var newPageNumber = pageNumber - 1;

			if (newPageNumber < 0)
			{
				newPageNumber = 0;
			}

			var pageItems = source
				.Skip(newPageNumber * pageSize)
				.Take(pageSize)
				.ToList();

			var hasNextPage = ((pageNumber + 1) * pageSize) < allItemsCount;

			return new PageableList<TSource>
			{
				HasNext = hasNextPage,
				HasPrevious = pageNumber > 0,
				Items = pageItems,
                PageInfo = new PageInfo
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
				TotalCount = allItemsCount
			};
		}

		public static void Add<TModel, TDataContext>(this TDataContext dataContext, TModel model)
			where TDataContext : DbContext
			where TModel : class
		{
			if (model != null)
			{
				dataContext.Set<TModel>().Add(model);
			}
		}

		public static void Delete<TModel, TDataContext>(this TDataContext dataContext, TModel model)
			where TDataContext : DbContext
			where TModel : class
		{
			if (model != null)
			{
				dataContext.Set<TModel>().Remove(model);
			}
		}

		public static void Update<TModel, TDataContext>(this TDataContext dataContext, TModel model)
			where TDataContext : DbContext
			where TModel : class, IHasIdentifyKey
		{
			if (model != null)
			{
				var entry = dataContext.Entry(model);

				TModel currentValue = null;

				if (entry.State == EntityState.Detached)
				{
					currentValue = dataContext.Set<TModel>().Find(model.Id);
				}

				if (currentValue != null)
				{
					var attachedEntry = dataContext.Entry(currentValue);

					attachedEntry.CurrentValues.SetValues(model);
				}
				else
				{
					dataContext.Entry(model).State = EntityState.Modified;
				}
			}
		}

		public static void Add<TModel, TDataContext>(this TDataContext dataContext, IEnumerable<TModel> models)
			where TDataContext : DbContext
			where TModel : class
		{
			foreach (var model in models)
			{
				dataContext.Add(model);
			}
		}

		public static void Delete<TModel, TDataContext>(this TDataContext dataContext, List<TModel> models)
			where TDataContext : DbContext
			where TModel : class
		{
			foreach (var model in models)
			{
				dataContext.Delete(model);
			}
		}

		public static void Update<TModel, TDataContext>(this TDataContext dataContext, IEnumerable<TModel> models)
			where TDataContext : DbContext
			where TModel : class, IHasIdentifyKey
		{
			foreach (var model in models)
			{
				dataContext.Update(model);
			}
		}
	}
}
