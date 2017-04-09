using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IProjectsRepository : IRepositoryBase<Project>
    {

        List<Project> GetUserProjects(int userId, int limit, int offset, string searchString, string sortingPropertyName, bool desc = false);

        int GetUserProjectsCount(int userId, string searchString);

        List<Project> GetUserProjectParticipations(int userId, int limit, int offset, string searchString, string sortingPropertyName, bool desc = false);

        int GetUserProjectParticipationsCount(int userId, string searchString);

        Project GetProjectWithData(int id);

        void DeleteProject(Project project);

        List<Group> GetGroups(int lecturerId = 0);
    }
}
