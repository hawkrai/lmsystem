using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IBugsRepository : IRepositoryBase<Bug>
    {
        List<Bug> GetUserBugs(int userId, int limit, int offset, string searchString, string sortedPropertyName, bool desc = false);

        int GetUserBugsCount(int userId, string searchString);

        Bug SaveBug(Bug bug);
        
        void DeleteBug(Bug bug);
    }
}
