using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
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

    public void AddGroup(Group @group)
    {
      using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
      {
        repositoriesContainer.GroupsRepository.Save(@group);
        repositoriesContainer.ApplyChanges();
      }
    }
  }
}
