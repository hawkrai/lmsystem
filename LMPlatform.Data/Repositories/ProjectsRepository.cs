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

        public void DeleteProject(Project project)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var model = context.Set<Project>().FirstOrDefault(e => e.Id == project.Id);
                context.Delete(model);

                context.SaveChanges();
            }
        }
    }
}
