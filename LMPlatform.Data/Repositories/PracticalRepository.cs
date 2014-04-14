namespace LMPlatform.Data.Repositories
{
    using Application.Core.Data;

    using LMPlatform.Data.Infrastructure;
    using LMPlatform.Data.Repositories.RepositoryContracts;
    using LMPlatform.Models;

    public class PracticalRepository : RepositoryBase<LmPlatformModelsContext, Practical>, IPracticalRepository
    {
        public PracticalRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {
        }
    }
}