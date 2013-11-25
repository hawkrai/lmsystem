using System.Collections.Generic;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
  using Application.Core.Data;

  using LMPlatform.Models;

  public interface IGroupsRepository : IRepositoryBase<Group>
  {
    Group GetGroup(int id);

    List<Group> GetGroups();
  }
}