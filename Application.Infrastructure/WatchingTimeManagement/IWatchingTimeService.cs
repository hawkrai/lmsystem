using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure.WatchingTimeManagement
{
    public interface IWatchingTimeService
    {
        void SaveWatchingTime(WatchingTime item);
        List<WatchingTime> GetAllRecords(int conceptId);
        WatchingTime GetByConceptSubject(int conceptId, int userId);

    }
}
