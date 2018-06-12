using Application.Core.Data;
using LMPlatform.Models;
using System.Collections.Generic;

namespace Application.Infrastructure.BugManagement
{
    public interface IBugManagementService
    {
        List<Bug> GetUserBugs(int userId, int pageSize, int pageNumber, string sortingPropertyName, bool desc, string searchString);
        int GetUserBugsCount(int userId, string searchString);
        List<Bug> GetProjectBugs(int projectId, int pageSize, int pageNumber, string sortingPropertyName, bool desc, string searchString);
        int GetProjectBugsCount(int projectId, string searchString);

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
