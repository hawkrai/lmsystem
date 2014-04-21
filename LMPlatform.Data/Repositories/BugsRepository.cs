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

        public void DeleteBug(Bug bug)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var model = context.Set<Bug>().FirstOrDefault(e => e.Id == bug.Id);
                context.Delete(model);

                context.SaveChanges();
            }
        }
    }
}
