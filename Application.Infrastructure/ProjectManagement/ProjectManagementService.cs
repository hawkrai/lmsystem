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

        public List<Project> GetProjects()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectsRepository.GetAll(new Query<Project>().Include(e => e.Creator)).ToList();
            }
        }

        public IPageableList<Project> GetAllProjects(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
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

        public IPageableList<Project> GetChosenProjects(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Project>(e => e.IsChosen == true);
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

        public void SaveProject(Project project)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.ProjectsRepository.Save(project);
                repositoriesContainer.ApplyChanges();
            }
        }
    }
}
