using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Models;

namespace Application.Infrastructure.GroupManagement
{
  public interface IGroupManagementService
  {
    Group GetGroup(int groupId);

    List<Group> GetGroups(IQuery<Group> query = null);

    void AddGroup(Group group);
  }
}