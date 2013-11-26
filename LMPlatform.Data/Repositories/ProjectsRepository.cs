using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class ProjectsRepository : RepositoryBase<LmPlatformModelsContext, Project>, IProjectsRepository
    {
        public ProjectsRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }

        public Project GetProject(int id)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var project = context.Projects.FirstOrDefault(p => p.Id == id);
                return project;
            }
        }

        public List<Project> GetProjects()
        {
            using (var context = new LmPlatformModelsContext())
            {
                var projects = context.Projects.ToList();
                return projects;
            }
        }
    }
}
