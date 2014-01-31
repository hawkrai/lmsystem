using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
	public class SubGroupRepository : RepositoryBase<LmPlatformModelsContext, SubGroup>, ISubGroupRepository
	{
		public SubGroupRepository(LmPlatformModelsContext dataContext) : base(dataContext)
		{
		}
	}
}