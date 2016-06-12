using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using System.Linq;

namespace LMPlatform.Data.Repositories
{
    public class ModulesRepository : RepositoryBase<LmPlatformModelsContext, Module>, IModulesRepository
    {
        public ModulesRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }

        protected override IQueryable<Module> PerformGetAll(IQuery<Module> query, LmPlatformModelsContext dataContext)
        {
            var updatedQuery = query.Include(q=>q.SubjectModules);
            return base.PerformGetAll(updatedQuery, dataContext);
        }
    }
}