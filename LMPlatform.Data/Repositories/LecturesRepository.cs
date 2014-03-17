using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class LecturesRepository : RepositoryBase<LmPlatformModelsContext, Lectures>, ILecturesRepository
    {
        public LecturesRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }
    }
}