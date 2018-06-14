using LMPlatform.Models.BTS;
using System.Collections.Generic;

namespace Application.Infrastructure.BTS
{
    public interface IMatrixManagmentService
    {
        void Fillrequirements(int projectId, string requirementsFileName);

        List<ProjectMatrixRequirement> GetRequirements(int projectId);
    }
}
