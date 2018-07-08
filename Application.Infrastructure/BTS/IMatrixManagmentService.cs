using LMPlatform.Models.BTS;
using System.Collections.Generic;

namespace Application.Infrastructure.BTS
{
    public interface IMatrixManagmentService
    {
        void FillRequirements(int projectId, string requirementsFileName);

        List<ProjectMatrixRequirement> GetRequirements(int projectId);

        void ClearRequirements(int projectId);

        void FillRequirementsCoverage(int projectId, string testsFileName);
    }
}
