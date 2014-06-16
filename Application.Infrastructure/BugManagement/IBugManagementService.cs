using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;
using LMPlatform.Models;

namespace Application.Infrastructure.BugManagement
{
    public interface IBugManagementService
    {
        Bug GetBug(int bugId);

        List<Bug> GetBugs();

        IPageableList<Bug> GetAllBugs(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        List<BugLog> GetBugLogs(int bugId);

        Bug SaveBug(Bug bug);

        BugLog SaveBugLog(BugLog bugLog);

        void DeleteBug(int bugId);

        void ClearBugLogs(int bugId);
    }
}
