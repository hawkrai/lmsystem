using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class ModulesRepository : RepositoryBase<LmPlatformModelsContext, Module>, IModulesRepository
    {
        public ModulesRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }
    }
}