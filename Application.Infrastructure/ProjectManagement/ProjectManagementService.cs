using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories;
using LMPlatform.Data.Repositories.RepositoryContracts;
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

        public void AssingRole(int userId, int projectId, int roleId)
        {
            var projectUser = new ProjectUser
            {
                UserId = userId,
                ProjectId = projectId,
                ProjectRoleId = roleId
            };
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

        public void SaveProject(Project project)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.ProjectsRepository.Save(project);
                repositoriesContainer.ApplyChanges();
            }
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
    }
}
