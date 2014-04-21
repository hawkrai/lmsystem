using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class ProjectCommentsRepository : RepositoryBase<LmPlatformModelsContext, ProjectComment>, IProjectCommentsRepository
    {
        public ProjectCommentsRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }
    }
}
