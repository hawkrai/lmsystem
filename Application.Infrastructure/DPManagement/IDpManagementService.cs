using System.Collections.Generic;
using Application.Infrastructure.DTO;
using LMPlatform.Models;
using LMPlatform.Models.DP;

namespace Application.Infrastructure.DPManagement
{
    public interface IDpManagementService
    {
        List<DiplomProjectData> GetProjects(int page, int count, out int total);

        DiplomProjectData GetProject(int id);

        void SaveProject(DiplomProjectData projectData);

        void DeleteProject(int id);
    }
}
