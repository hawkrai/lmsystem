using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Application.Core.Data
{
    public class RepositoryBase<TDataContext, TModel> : IRepositoryBase<TModel>, IRepositoryBaseExtended<TModel>
        where TModel : ModelBase
        where TDataContext : DbContext, new()
    {
        #region RepositoryBase Members

        public virtual void Add(TModel model)
        {
            if (model != null)
            {
                using (var dbContext = new TDataContext())
                {
                    model = PerformAdd(model, dbContext);
                    OnAdded(model, dbContext);

                    dbContext.SaveChanges();
                }
            }
        }

        public virtual void Add(IEnumerable<TModel> models)
        {
            foreach (var model in models)
            {
                Add(model);
            }
        }

        public virtual void Delete(TModel model)
        {
            if (model != null)
            {
                using (var dbContext = new TDataContext())
                {
                    OnDeleting(model, dbContext);

                    PerformDelete(model, dbContext);

                    OnDeleted(model, dbContext);

                    dbContext.SaveChanges();
                }
            }
        }

        public virtual void Delete(IEnumerable<TModel> models)
        {
            foreach (var model in models)
            {
                Delete(model);
            }
        }

        public virtual IEnumerable<TModel> GetAll()
        {
            using (var dbContext = new TDataContext())
            {
                return PerformSelectAll(dbContext).ToList();
            }
        }

        public virtual TModel GetSingle(TModel model)
        {
            using (var dbContext = new TDataContext())
            {
                var dataModel = PerformSelectSingle(model, dbContext);

                return dataModel;
            }
        }

        public virtual void Save(TModel model)
        {
            if (model.Id == 0)
            {
                Add(model);
            }
            else
            {
                Update(model);
            }
        }

        public virtual void Save(IEnumerable<TModel> models)
        {
            foreach (var model in models)
            {
                Save(model);
            }
        }

        public virtual void Update(TModel model)
        {
            if (model != null)
            {
                using (var dbContext = new TDataContext())
                {
                    var entry = dbContext.Entry(model);

                    TModel currentValue = null;

                    if (entry.State == EntityState.Detached)
                    {
                        currentValue = dbContext.Set<TModel>().Find(model.Id);
                    }

                    OnUpdating(model, currentValue, dbContext);

                    PerformUpdate(model, currentValue, dbContext);

                    OnUpdated(model, dbContext);

                    dbContext.SaveChanges();
                }
            }
        }

        public virtual void Update(IEnumerable<TModel> models)
        {
            foreach (var model in models)
            {
                Update(model);
            }
        }

        #endregion RepositoryBase Members

        #region Protected Members

        protected virtual void OnAdded(TModel model, DbContext context)
        {
        }

        protected virtual void OnDeleted(TModel model, DbContext context)
        {
        }

        protected virtual void OnDeleting(TModel model, DbContext context)
        {
        }

        protected virtual void OnUpdated(TModel model, DbContext context)
        {
        }

        protected virtual void OnUpdating(TModel newValue, TModel currentValue, DbContext context)
        {
        }

        protected virtual TModel PerformAdd(TModel model, DbContext context)
        {
            context.Set<TModel>().Add(model);
            context.SaveChanges();

            return model;
        }

        protected virtual void PerformDelete(TModel model, DbContext context)
        {
            var deletingEntity = context.Set<TModel>().Find(model.Id);

            if (deletingEntity != null)
            {
                context.Set<TModel>().Remove(deletingEntity);
            }
        }

        protected virtual IEnumerable<TModel> PerformSelectAll(DbContext context)
        {
            return context.Set<TModel>();
        }

        protected virtual TModel PerformSelectSingle(TModel model, DbContext context)
        {
            var dataModel = context.Set<TModel>().Find(model.Id);

            return dataModel;
        }

        protected virtual void PerformUpdate(TModel newValue, TModel currentValue, DbContext context)
        {
            if (currentValue != null)
            {
                var attachedEntry = context.Entry(currentValue);

                attachedEntry.CurrentValues.SetValues(newValue);
            }
            else
            {
                context.Entry(newValue).State = EntityState.Modified;
            }
        }

        #endregion Protected Members
    }
}
