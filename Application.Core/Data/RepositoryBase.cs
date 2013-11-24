using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataException = System.Data.DataException;

namespace Application.Core.Data
{
	public class RepositoryBase<TDataContext, TModel> : IRepositoryBase<TModel>
		where TModel : ModelBase, new()
		where TDataContext : DbContext
	{
		private readonly TDataContext _dataContext;

		public RepositoryBase(TDataContext dataContext)
		{
			_dataContext = dataContext;
		}

		protected TDataContext DataContext
		{
			get { return _dataContext; }
		}

		protected void Add(TModel model)
		{
			ProcessMethod(() => PerformAdd(model, DataContext));
		}

		protected void Add(IEnumerable<TModel> models)
		{
			ProcessMethod(() =>
			{
				foreach (var model in models)
				{
					PerformAdd(model, DataContext);
				}
			});
		}

		public void Delete(TModel model)
		{
			ProcessMethod(() => PerformDelete(model, DataContext));
		}

		public void Delete(IEnumerable<TModel> models)
		{
			ProcessMethod(() =>
			{
				foreach (var model in models)
				{
					PerformDelete(model, DataContext);
				}
			});
		}

		public IPageableList<TModel> GetPageableBy(IPageableQuery<TModel> query = null)
		{
			return ProcessMethod(() =>
			{
				if (query == null)
				{
					query = new PageableQuery<TModel>(new PageInfo());
				}

				var items = PerformGetBy(query, DataContext).ToPageableList(query.PageInfo.PageNumber, query.PageInfo.PageSize);

				return items;
			});
		}

		public IList<TModel> GetAll(IQuery<TModel> query = null)
		{
			return ProcessMethod(() => PerformGetAll());
		}

		protected virtual IList<TModel> PerformGetAll()
		{
			return DataContext.Set<TModel>().ToList();
		}

        public TModel GetBy(IQuery<TModel> query)
        {
            return ProcessMethod(() =>
            {
                var item = PerformSelectSingle(query, DataContext);
                return item;
            });
        }

		public void Save(TModel model, Func<TModel, bool> performUpdate = null)
		{
			if (model != null)
			{
				if (performUpdate == null)
				{
					performUpdate = e => !e.IsNew;
				}

				if (performUpdate(model))
				{
					Update(model);
				}
				else
				{
					Add(model);
				}
			}
		}

		public void Save(IEnumerable<TModel> models, Func<TModel, bool> performUpdate = null)
		{
			ProcessMethod(() =>
			{
				foreach (var model in models)
				{
					if (model != null)
					{
						if (performUpdate == null)
						{
							performUpdate = e => !e.IsNew;
						}

						if (performUpdate(model))
						{
							PerformUpdate(model, _dataContext);
						}
						else
						{
							PerformAdd(model, _dataContext);
						}
					}
				}
			});
		}

		protected void Update(TModel model)
		{
			ProcessMethod(() =>
			{
				if (model != null)
				{
					PerformUpdate(model, _dataContext);
				}
			});
		}

		protected void Update(IEnumerable<TModel> models)
		{
			ProcessMethod(() =>
			{
				foreach (var model in models)
				{
					if (model != null)
					{
						PerformUpdate(model, _dataContext);
					}
				}
			});
		}

		protected virtual void PerformAdd(TModel model, TDataContext dataContext)
		{
			dataContext.Set<TModel>().Add(model);
		}

		protected virtual void PerformDelete(TModel model, TDataContext dataContext)
		{
			var deletingEntity = dataContext.Set<TModel>().Find(model.Id);

			if (deletingEntity != null)
			{
				dataContext.Set<TModel>().Remove(deletingEntity);
			}
		}

		protected virtual IOrderedQueryable<TModel> PerformGetBy(IPageableQuery<TModel> query, TDataContext dataContext)
		{
			var sequence = ApplyFilters(query, dataContext.Set<TModel>());

			sequence = ApplyIncludedProperties(query, sequence);

			sequence = ApplySortCriterias(query, sequence);

			return sequence as IOrderedQueryable<TModel>;
		}

		protected virtual TModel PerformSelectSingle(IQuery<TModel> query, TDataContext dataContext)
		{
			var sequence = ApplyFilters(query, dataContext.Set<TModel>());
			sequence = ApplyIncludedProperties(query, sequence);

			return sequence.SingleOrDefault();
		}

		protected virtual void PerformUpdate(TModel newValue, TDataContext dataContext)
		{
			dataContext.Update(newValue);
		}

		protected TResult ProcessMethod<TResult>(Func<TResult> func)
		{
			TResult result;

			try
			{
				result = func();
			}
			catch (Exception exception)
			{
				throw new DataException("Ошибка уровня доступа к данным", exception);
			}

			return result;
		}

		protected void ProcessMethod(Action action)
		{
			try
			{
				action();
			}
			catch (Exception exception)
			{
				throw new DataException("При сохранении данных возникла ошибка.", exception);
			}
		}

		protected IQueryable<TModel> ApplyFilters(IQuery<TModel> searchCritery, IQueryable<TModel> sequence)
		{
			if (searchCritery.Filters != null && searchCritery.Filters.Any())
			{
				sequence = searchCritery.Filters.Aggregate(sequence, (current, filterClause) => current.Where(filterClause));
			}

			return sequence;
		}

		protected IQueryable<TModel> ApplyIncludedProperties(IQuery<TModel> searchCritery, IQueryable<TModel> sequence)
		{
			return searchCritery.IncludedProperties.Aggregate(sequence, 
				(current, includeProperty) => current.Include(includeProperty));
		}

		protected IOrderedQueryable<TModel> ApplySortCriterias(IPageableQuery<TModel> searchCritery, 
			IQueryable<TModel> sequence)
		{
			var result = sequence.OrderBy(e => e.Id);

			return searchCritery.SortCriterias.Aggregate(result, 
				(current, sortCriteria) =>
					sortCriteria.SortDirection == SortDirection.Asc
						? current.OrderByAsc(sortCriteria.Name)
						: current.OrderByDesc(sortCriteria.Name));
		}
	}
}