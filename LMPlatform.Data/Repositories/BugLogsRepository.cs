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
    public class BugLogsRepository : RepositoryBase<LmPlatformModelsContext, BugLog>, IBugLogsRepository
    {
        public BugLogsRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }

        public void DeleteBugLog(BugLog bugLog)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var model = context.Set<BugLog>().FirstOrDefault(e => e.Id == bugLog.Id);
                context.Delete(model);

                context.SaveChanges();
            }
        }
    }
}
