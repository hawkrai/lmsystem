using System.Collections.Generic;
using System.Linq;

namespace LMPlatform.Data.Repositories
{
  using Application.Core.Data;

  using LMPlatform.Data.Infrastructure;
  using LMPlatform.Data.Repositories.RepositoryContracts;
  using LMPlatform.Models;

  public class GroupsRepository : RepositoryBase<LmPlatformModelsContext, Group>, IGroupsRepository
  {
    public GroupsRepository(LmPlatformModelsContext dataContext)
      : base(dataContext)
    {
    }

    public Group GetGroup(int id)
    {
      using (var context = new LmPlatformModelsContext())
      {
        var group = context.Groups.FirstOrDefault(g => g.Id == id);
        return group;
      }
    }

    public List<Group> GetGroups()
    {
      using (var context = new LmPlatformModelsContext())
      {
        var groups = context.Groups.ToList();
        return groups;
      }
    }
  }
}