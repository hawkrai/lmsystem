using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.Data.Repositories
{
    public class TestsRepository : RepositoryBase<LmPlatformModelsContext, Test>, ITestsRepository
    {
        public TestsRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }
    }
}
