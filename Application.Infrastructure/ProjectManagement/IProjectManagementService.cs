using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;
using LMPlatform.Models;

namespace Application.Infrastructure.ProjectManagement
{
    public interface IProjectManagementService
    {
        List<Project> GetUserProjects(int userId, int pageSize, int pageNumber, string searchString = null);

        int GetUserProjectsCount(int userId, string searchString = null);
        Project GetProject(int projectId, bool includeBugs = false, bool includeUsers = false);

        List<Project> GetProjects();

        List<ProjectComment> GetProjectComments(int projectId);

        IPageableList<Project> GetProjects(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        IPageableList<Project> GetUserProjects(int userId, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);
        
        List<ProjectUser> GetProjectUsers(int projectId);

        List<ProjectUser> GetProjectsOfUser(int userId);

        ProjectUser GetProjectUser(int projectUserId);
            
        IPageableList<ProjectUser> GetProjectUsers(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        void SaveComment(ProjectComment comment);
        
        Project SaveProject(Project project);

        void UpdateProject(Project project);

        void DeleteProject(int projectId);

        void ClearProject(int projectId);

        void DeleteProjectUser(int projectUserId);

        void AssingRole(ProjectUser projectUser);

        bool IsUserAssignedOnProject(int userId, int projectId);

        string GetCreatorName(int id);

        void DeleteUserFromProject(int userId, int projectId);
    }
}
