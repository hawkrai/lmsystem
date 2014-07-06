using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using Application.Core.Data;

namespace Application.Core.Extensions
{
    public static class EnumerableExtendedExtensions
    {
        public static IEnumerable<T> Descendents<T>(
            this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (T item in source)
            {
                yield return item;

                foreach (T subItem in Descendents(selector(item), selector))
                {
                    yield return subItem;
                }
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
           Func<TSource, TKey> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            return _DistinctByInternal(source, keySelector);
        }

        private static IEnumerable<TSource> _DistinctByInternal<TSource, TKey>(IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(g => g.First());
        }

        public static IOrderedEnumerable<T> OrderByAsc<T>(this IEnumerable<T> source, string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            return source.OrderBy(e => propertyInfo.GetValue(e, null));
        }

        public static IOrderedEnumerable<T> OrderByDesc<T>(this IEnumerable<T> source, string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            return source.OrderByDescending(e => propertyInfo.GetValue(e, null));
        }

        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, Func<T, TKey> getKey)
        {
            return from item in items
                   join otherItem in other on getKey(item)
                   equals getKey(otherItem) into tempItems
                   from temp in tempItems.DefaultIfEmpty()
                   where ReferenceEquals(null, temp) || temp.Equals(default(T))
                   select item;
        }

        public static PagedList<T> ApplyPaging<T>(this IQueryable<T> query, GetPagedListParams parms)
        {
            return
                new PagedList<T>
                {
                    Total = query.Count(),
                    Items =
                        query
                        .OrderBy(parms.SortExpression)
                        .Skip((parms.Page - 1) * parms.PageSize)
                        .Take(parms.PageSize)
                        .ToList()
                };
        }
    }
}
