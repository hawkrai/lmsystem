using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.Data.Repositories
{
    public class TestUnlockRepository : RepositoryBase<LmPlatformModelsContext, TestUnlock>, ITestUnlocksRepository
    {
        public TestUnlockRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }
    }
}
