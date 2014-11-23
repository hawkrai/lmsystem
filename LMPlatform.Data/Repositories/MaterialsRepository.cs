using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class MaterialsRepository : RepositoryBase<LmPlatformModelsContext, Materials>, IMaterialsRepository
    {
        public MaterialsRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {
        }
    }
}
