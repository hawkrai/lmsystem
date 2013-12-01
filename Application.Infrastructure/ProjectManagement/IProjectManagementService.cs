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

        IPageableList<Project> GetAllProjects(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        IPageableList<Project> GetChosenProjects(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        void SaveProject(Project project);
    }
}
