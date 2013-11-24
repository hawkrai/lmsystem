using System;

namespace Application.Core.Data
{
    public interface IRepositoriesContainer : IDisposable
    {
        IRepositoryBase<TModel> RepositoryFor<TModel>() where TModel : ModelBase, new();

        void ApplyChanges();
    }
}