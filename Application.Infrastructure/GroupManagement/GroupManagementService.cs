using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace Application.Infrastructure.GroupManagement
{
  public class GroupManagementService : IGroupManagementService
  {
    private readonly LazyDependency<IGroupsRepository> _groupsRepository = new LazyDependency<IGroupsRepository>();

    public IGroupsRepository GroupsRepository
    {
      get
      {
        return _groupsRepository.Value;
      }
    }

    public Group GetGroup(int groupId)
    {
      return GroupsRepository.GetGroup(groupId);
    }

    public List<Group> GetGroups()
    {
      return GroupsRepository.GetGroups();
    }
  }
}
