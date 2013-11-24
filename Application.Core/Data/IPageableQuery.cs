using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Core.Data
{
    public interface IPageableQuery<TModel> : IQuery<TModel>
    {
        IPageInfo PageInfo
        {
            get;
        }

	    new IPageableQuery<TModel> Include<TResult>(Expression<Func<TModel, TResult>> includeExpression);

		new IPageableQuery<TModel> AddFilterClause(Expression<Func<TModel, bool>> filterClause);

        IList<ISortCriteria> SortCriterias
        {
            get;
        }

        IPageableQuery<TModel> OrderBy(IEnumerable<ISortCriteria> sortCriterias);
    }
}
