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
        Project GetProject(int projectId);

        List<Project> GetProjects();

        List<ProjectComment> GetProjectComments(int projectId);

        IPageableList<Project> GetProjects(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        List<ProjectUser> GetProjectUsers(int projectId); 
            
        IPageableList<ProjectUser> GetProjectUsers(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        void SaveComment(ProjectComment comment);
        
        void SaveProject(Project project);

        void UpdateProject(Project project);

        void DeleteProject(int projectId);

        void DeleteProjectUser(int projectUserId);

        void AssingRole(int userId, int projectId, int roleId);
    }
}
