using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMPlatform.Models;
using LMPlatform.Data.Repositories;
using Application.Core.Data;

namespace Application.Infrastructure.WatchingTimeManagement
{
    public class WatchingTimeService : IWatchingTimeService
    {
        public void SaveWatchingTime(WatchingTime item)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var watchingTime = repositoriesContainer.WatchingTimeRepository.GetByUserConceptIds(item.UserId, item.ConceptId);
                //var watchingTime = repositoriesContainer.WatchingTimeRepository.GetByUserConceptIds(item.UserId,item.Concept.Id);
                if (watchingTime != null)
                {
                    watchingTime.Time += item.Time;
                    repositoriesContainer.WatchingTimeRepository.Save(watchingTime);
                }
                else
                {
                    repositoriesContainer.WatchingTimeRepository.Save(item);
                }
                repositoriesContainer.ApplyChanges();
            }
        }

        public WatchingTime GetByConceptSubject(int conceptId, int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.WatchingTimeRepository.GetAll(new Query<WatchingTime>().AddFilterClause(u => u.ConceptId == conceptId & u.UserId == userId)).ToList()[0];
                //return repositoriesContainer.WatchingTimeRepository.GetAll(new Query<WatchingTime>().AddFilterClause(u => u.Concept.Id == conceptId & u.UserId == userId)).ToList()[0];
            }
        }

        public List<WatchingTime> GetAllRecords(int conceptId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.WatchingTimeRepository.GetAll(new Query<WatchingTime>().AddFilterClause(u => u.ConceptId == conceptId)).ToList();
                //return repositoriesContainer.WatchingTimeRepository.GetAll(new Query<WatchingTime>().AddFilterClause(u => u.Concept.Id == conceptId)).ToList();
            }
        }
    }
}
