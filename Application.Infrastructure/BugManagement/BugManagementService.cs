using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

namespace Application.Infrastructure.BugManagement
{
    public class BugManagementService : IBugManagementService
    {
        public Bug GetBug(int bugId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.BugsRepository.GetBy(new Query<Bug>(e => e.Id == bugId));
            }
        }

        public List<Bug> GetBugs()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.BugsRepository.GetAll().ToList();
            }
        }

        public IPageableList<Bug> GetAllBugs(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Bug>(pageInfo);
            query.Include(e => e.Status);
            query.Include(e => e.Severity);
            query.Include(e => e.Symptom);
            query.Include(e => e.Project);
            if (!string.IsNullOrEmpty(searchString))
            {
                query.AddFilterClause(
                    e => e.Summary.ToLower().StartsWith(searchString) || e.Summary.ToLower().Contains(searchString));
            }

            query.OrderBy(sortCriterias);
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.BugsRepository.GetPageableBy(query);
            }
        }
    }
}
