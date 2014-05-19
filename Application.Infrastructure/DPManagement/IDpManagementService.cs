using System.Collections.Generic;
using Application.Infrastructure.DTO;

namespace Application.Infrastructure.DPManagement
{
    public interface IDpManagementService
    {
        List<DiplomProjectData> GetProjects(int page, int count, out int total);

        DiplomProjectData GetProject(int id);

        void SaveProject(DiplomProjectData projectData);

        void DeleteProject(int userId, int id);

        void AssignProject(int userId, int projectId, int studentId);

        void DeleteAssignment(int userId, int id);

        List<StudentData> GetStudentsByDiplomProjectId(int diplomProjectId, int page, int count, out int total);
    }
}
