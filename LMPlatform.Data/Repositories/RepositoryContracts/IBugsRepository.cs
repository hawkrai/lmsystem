using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IBugsRepository : IRepositoryBase<Bug>
    {
        Bug SaveBug(Bug bug);
        
        void DeleteBug(Bug bug);
    }
}
