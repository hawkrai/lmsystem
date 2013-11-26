using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace Application.Infrastructure.ProjectManagement
{
    public class ProjectManagementService
    {
        private readonly LazyDependency<IProjectsRepository> _projectsRepository = new LazyDependency<IProjectsRepository>();

        public IProjectsRepository ProjectsRepository
        {
            get
            {
                return _projectsRepository.Value;
            }
        }

        public Project GetProject(int projectId)
        {
            return ProjectsRepository.GetProject(projectId);
        }

        public List<Project> GetProjects()
        {
            return ProjectsRepository.GetProjects();
        }
    }
}
