using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models.BTS;

namespace LMPlatform.Data.Repositories
{
    public class ProjectMatrixRequirementsRepository : RepositoryBase<LmPlatformModelsContext, ProjectMatrixRequirement>, IProjectMatrixRequirementsRepository
    {

        public ProjectMatrixRequirementsRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {
        }
    }
}
