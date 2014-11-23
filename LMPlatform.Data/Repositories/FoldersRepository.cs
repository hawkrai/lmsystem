using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class FoldersRepository : RepositoryBase<LmPlatformModelsContext, Folders>, IFoldersRepository
    {
        public FoldersRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {
        }
    }
}