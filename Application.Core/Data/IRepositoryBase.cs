using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Core.Data
{
    public interface IRepositoryBase<TModel> where TModel : IHasIdentifyKey
    {
        #region IRepositoryBase Members

        void Delete(TModel model);

        IPageableList<TModel> GetPageableBy(IPageableQuery<TModel> query = null);

        IQueryable<TModel> GetAll(IQuery<TModel> query = null);

        TModel GetBy(IQuery<TModel> query);

        void Save(TModel model, Func<TModel, bool> performUpdate = null);

        void Delete(IEnumerable<TModel> models);

        void Save(IEnumerable<TModel> models, Func<TModel, bool> performUpdate = null);

        #endregion IRepositoryBase Members
    }
}