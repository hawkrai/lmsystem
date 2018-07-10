using Application.Core.Data;
using LMPlatform.Models.BTS;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IProjectMatrixRequirementsRepository : IRepositoryBase<ProjectMatrixRequirement>
    {
        void DeleteAll(int projectId);
        void UpdateCoveredWhere(Query<ProjectMatrixRequirement> query);
    }
}
