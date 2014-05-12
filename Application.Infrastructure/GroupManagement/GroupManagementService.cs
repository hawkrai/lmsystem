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
                return repositoriesContainer.GroupsRepository.GetBy(new Query<Group>(e => e.Id == groupId).Include(e => e.Students.Select(x => x.LecturesVisitMarks)));
            }
        }

        public List<Group> GetGroups(IQuery<Group> query = null)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.GroupsRepository.GetAll(query).ToList();
            }
        }

        public IPageableList<Group> GetGroupsPageable(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Group>(pageInfo);

            if (!string.IsNullOrEmpty(searchString))
            {
                query.AddFilterClause(
                    e => e.Name.ToLower().StartsWith(searchString) || e.Name.ToLower().Contains(searchString));
            }

            query.OrderBy(sortCriterias).Include(g => g.Students);
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var groups = repositoriesContainer.GroupsRepository.GetPageableBy(query);
                return groups;
            }
        }

        public Group AddGroup(Group group)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.GroupsRepository.Save(group);
                repositoriesContainer.ApplyChanges();
            }

            return group;
        }

        public Group UpdateGroup(Group group)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.GroupsRepository.Save(group);
                repositoriesContainer.ApplyChanges();
            }

            return group;
        }

        public void DeleteGroup(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var group = repositoriesContainer.GroupsRepository.GetBy(new Query<Group>().AddFilterClause(g => g.Id == id));
                repositoriesContainer.GroupsRepository.Delete(group);
                repositoriesContainer.ApplyChanges();
            }
        }

        public Group GetGroupByName(string groupName)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var group = repositoriesContainer.GroupsRepository.GetBy(new Query<Group>().AddFilterClause(g => g.Name == groupName));

                return group;
            }
        }
    }
}