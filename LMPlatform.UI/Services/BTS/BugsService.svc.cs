using System.Linq;
using Application.Core;
using Application.Infrastructure.BugManagement;
using LMPlatform.UI.Services.Modules.BTS;
using WebMatrix.WebData;
using LMPlatform.UI.Attributes;

namespace LMPlatform.UI.Services.BTS
{
    [JwtAuth(Roles = "student, lector")]
    public class BugsService : IBugsService
    {
        private readonly LazyDependency<IBugManagementService> bugManagementService = new LazyDependency<IBugManagementService>();

        public IBugManagementService BugManagementService => bugManagementService.Value;

        public BugsResult Index(int pageSize, int pageNumber, string sortingPropertyName, bool desc = false, string searchString = null)
        {
            var bugs = BugManagementService.GetUserBugs(WebSecurity.CurrentUserId, pageSize, pageNumber, sortingPropertyName, desc, searchString)
                .Select(e => new BugViewData(e)).ToList();
            var totalCount = BugManagementService.GetUserBugsCount(WebSecurity.CurrentUserId, searchString);
            return new BugsResult
            {
                Bugs = bugs,
                TotalCount = totalCount
            };
        }

        public BugsResult ProjectBugs(int projectId, int pageSize, int pageNumber, string sortingPropertyName, bool desc = false, string searchString = null)
        {
            var bugs = BugManagementService.GetProjectBugs(projectId, pageSize, pageNumber, sortingPropertyName, desc, searchString)
                .Select(e => new BugViewData(e)).ToList();
            var totalCount = BugManagementService.GetProjectBugsCount(projectId, searchString);
            return new BugsResult
            {
                Bugs = bugs,
                TotalCount = totalCount
            };
        }
    }
}
