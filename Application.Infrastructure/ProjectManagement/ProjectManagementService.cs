using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

namespace Application.Infrastructure.ProjectManagement
{
    public class ProjectManagementService : IProjectManagementService
    {
        public Project GetProject(int projectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectsRepository.GetBy(new Query<Project>(e => e.Id == projectId).Include(e => e.Creator));
            }
        }

        public List<ProjectUser> GetProjectUsers(int projectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return
                    repositoriesContainer.ProjectUsersRepository.GetAll(new Query<ProjectUser>(e => e.ProjectId == projectId).Include(e => e.User))
                        .ToList();
            }
        }

        public List<ProjectUser> GetProjectsOfUser(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectUsersRepository.GetAll(new Query<ProjectUser>(e => e.UserId == userId).Include(e => e.Project).Include(e => e.User).Include(e => e.ProjectRole)).ToList();
            }
        }

        public ProjectUser GetProjectUser(int projectUserId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectUsersRepository.GetBy(new Query<ProjectUser>(e => e.Id == projectUserId).Include(e => e.Project).Include(e => e.User).Include(e => e.ProjectRole));
            }
        }

        public List<ProjectComment> GetProjectComments(int projectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return
                    repositoriesContainer.ProjectCommentsRepository.GetAll(new Query<ProjectComment>(e => e.ProjectId == projectId).Include(e => e.User))
                        .ToList();
            }
        }

        public IPageableList<ProjectUser> GetProjectUsers(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<ProjectUser>(pageInfo);
            query.Include(e => e.User);
            if (!string.IsNullOrEmpty(searchString))
            {
                query.AddFilterClause(
                    e => e.User.FullName.ToLower().StartsWith(searchString) || e.User.FullName.ToLower().Contains(searchString));
            }

            query.OrderBy(sortCriterias);
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectUsersRepository.GetPageableBy(query);
            }
        }

        public List<Project> GetProjects()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectsRepository.GetAll(new Query<Project>().Include(e => e.Creator)).ToList();
            }
        }

        public IPageableList<Project> GetProjects(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Project>(pageInfo);
            query.Include(e => e.Creator);
            if (!string.IsNullOrEmpty(searchString))
			{
				query.AddFilterClause(
					e => e.Title.ToLower().StartsWith(searchString) || e.Title.ToLower().Contains(searchString));
			}

            query.OrderBy(sortCriterias);
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectsRepository.GetPageableBy(query);
            }
        }

        //public void AssingRole(int userId, int projectId, int roleId)
        public void AssingRole(ProjectUser projectUser)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.ProjectUsersRepository.Save(projectUser);
                repositoriesContainer.ApplyChanges();
            }
        }

        public void SaveComment(ProjectComment comment)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.ProjectCommentsRepository.Save(comment);
                repositoriesContainer.ApplyChanges();
            }
        }

        public Project SaveProject(Project project)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.ProjectsRepository.Save(project);
                repositoriesContainer.ApplyChanges();
            }

            return project;
        }

        public void UpdateProject(Project project)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.ProjectsRepository.Save(project);
                repositoriesContainer.ApplyChanges();
            }
        }

        public void DeleteProject(int projectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var project =
                    repositoriesContainer.ProjectsRepository.GetBy(
                        new Query<Project>().AddFilterClause(u => u.Id == projectId));
               repositoriesContainer.ProjectsRepository.DeleteProject(project);
            }
        }

        public void DeleteProjectUser(int projectUserId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var projectUser = repositoriesContainer.ProjectUsersRepository.GetBy(new Query<ProjectUser>().AddFilterClause(u => u.Id == projectUserId));
                repositoriesContainer.ProjectUsersRepository.DeleteProjectUser(projectUser);
            }
        }

        public bool IsUserAssignedOnProject(int userId, int projectId)
        {
            var isAssigned = false;
            var projectUsers = new ProjectManagementService().GetProjectUsers(projectId);
            foreach (var user in projectUsers)
            {
                if (user.UserId == userId)
                {
                    isAssigned = true;
                }
            }

            return isAssigned;
        }
    }
}
