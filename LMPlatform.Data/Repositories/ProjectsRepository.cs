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

        //public List<Project> GetProjects()
        //{
        //    using (var context = new LmPlatformModelsContext())
        //    {
                
        //    }
        //}
    }
}
