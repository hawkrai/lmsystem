using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMPlatform.Models;

namespace Application.Infrastructure.ProjectManagement
{
    public interface IProjectManagementService
    {
        Project GetProject(int projectId);

        List<Project> GetProjects();
    }
}
