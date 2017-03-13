using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IProjectsRepository : IRepositoryBase<Project>
    {

        List<Project> GetUserProjects(int userId, int limit, int offset);
        void DeleteProject(Project project);

        List<Group> GetGroups(int lecturerId = 0);
    }
}
