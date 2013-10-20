using System.Collections.Generic;

namespace Application.Core.Data
{
	public interface IRepositoryBase<TModel>
	{
		#region IRepositoryBase Members

		void Add(TModel model);

		void Delete(TModel model);

		IEnumerable<TModel> GetAll();

		TModel GetSingle(TModel model);

		void Save(TModel model);

		void Update(TModel model);

		#endregion IRepositoryBase Members
	}
}