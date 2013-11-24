using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Core.Data
{
    public class PageableQuery<TModel> : Query<TModel>, IPageableQuery<TModel>
    {
        private IPageInfo _pageInfo;
        private readonly IList<ISortCriteria> _sortCriterias = new List<ISortCriteria>();

        public IPageInfo PageInfo
        {
            get { return _pageInfo ?? (_pageInfo = new PageInfo()); }
        }

        public IList<ISortCriteria> SortCriterias
        {
            get
            {
                return _sortCriterias;
            }
        }

        public IPageableQuery<TModel> OrderBy(IEnumerable<ISortCriteria> sortCriterias)
        {
            if (sortCriterias != null)
            {
                foreach (var sortCriteria in sortCriterias)
                {
                    _sortCriterias.Add(sortCriteria);
                }
            }

            return this;
        }

        public IPageableQuery<TModel> OrderBy(ISortCriteria sortCriteria)
        {
            _sortCriterias.Add(sortCriteria);

            return this;
        }

        public PageableQuery(IPageInfo pageInfo, params Expression<Func<TModel, bool>>[] filterClauses)
            : base(filterClauses)
        {
            _pageInfo = pageInfo;
        }

        public PageableQuery(params Expression<Func<TModel, bool>>[] filterClauses)
            : base(filterClauses)
        {
            _pageInfo = new PageInfo();
        }

		public new IPageableQuery<TModel> AddFilterClause(Expression<Func<TModel, bool>> filterClause)
		{
			base.AddFilterClause(filterClause);

			return this;
		}

        public new IPageableQuery<TModel> Include<TResult>(Expression<Func<TModel, TResult>> includeExpression)
        {
            base.Include(includeExpression);

            return this;
        }
    }
}
