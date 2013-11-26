using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class BugsRepository : RepositoryBase<LmPlatformModelsContext, Bug>, IBugsRepository
    {
        public BugsRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {      
        }

        public Bug GetBug(int id)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var bug = context.Bugs.FirstOrDefault(b => b.Id == id);
                return bug;
            }
        }

        public List<Bug> GetBugs()
        {
            using (var context = new LmPlatformModelsContext())
            {
                var bugs = context.Bugs.ToList();
                return bugs;
            }
        }
    }
}
