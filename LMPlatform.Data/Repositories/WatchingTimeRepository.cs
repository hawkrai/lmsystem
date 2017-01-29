using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMPlatform.Data.Repositories
{
    public class WatchingTimeRepository : RepositoryBase<LmPlatformModelsContext, WatchingTime>, IWatchingTimeRepository
    {
        public WatchingTimeRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {
        }

        public void Create(WatchingTime watchingTime)
        {
            using (var context = new LmPlatformModelsContext())
            {
                context.WatchingTime.Add(watchingTime);
            }
        }

        public WatchingTime GetByUserConceptIds(int UserId, int conceptId)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var watchingTime = context.Set<WatchingTime>().FirstOrDefault(e => e.UserId == UserId & e.ConceptId == conceptId);
                //var watchingTime = context.Set<WatchingTime>().FirstOrDefault(e => e.UserId == UserId & e.Concept.Id == conceptId);
                return watchingTime;
            }
        }
    }
}
