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

namespace Application.Infrastructure.GroupManagement
{
    public class GroupManagementService : IGroupManagementService
    {
        public Group GetGroup(int groupId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.GroupsRepository.GetBy(new Query<Group>(e => e.Id == groupId));
            }
        }

        public List<Group> GetGroups()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.GroupsRepository.GetAll().ToList();
            }
        }
    }
}
