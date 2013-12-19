using System.Collections.Generic;
using System.Linq;
using LMPlatform.Models;

namespace Application.Infrastructure.GroupManagement
{
  public interface IGroupManagementService
  {
    Group GetGroup(int groupId);

    List<Group> GetGroups();

    void AddGroup(Group group);
  }
}