using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Data.Repositories.RepositoryContracts;
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
    }
}
