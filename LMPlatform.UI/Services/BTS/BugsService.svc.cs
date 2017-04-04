using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Application.Core;
using Application.Infrastructure.BugManagement;
using LMPlatform.UI.Services.Modules.BTS;
using WebMatrix.WebData;
using System.Web.Http;

namespace LMPlatform.UI.Services.BTS
{
    [Authorize(Roles = "student, lector")]
    public class BugsService : IBugsService
    {
        private readonly LazyDependency<IBugManagementService> bugManagementService = new LazyDependency<IBugManagementService>();

        public IBugManagementService BugManagementService
        {
            get
            {
                return bugManagementService.Value;
            }
        }

        public BugsResult Index(int pageSize, int pageNumber, string sortingPropertyName, bool desc = false, string searchString = null)
        {
            var bugs = BugManagementService.GetUserBugs(WebSecurity.CurrentUserId, pageSize, pageNumber, sortingPropertyName, desc, searchString).Select(e => new BugsViewData(e)).ToList();
            int totalCount = BugManagementService.GetUserBugsCount(WebSecurity.CurrentUserId, searchString);
            return new BugsResult
            {
                Bugs = bugs,
                TotalCount = totalCount
            };
        }
    }
}
