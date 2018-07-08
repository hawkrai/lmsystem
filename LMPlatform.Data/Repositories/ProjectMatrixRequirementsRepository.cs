using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models.BTS;
using System.Linq;

namespace LMPlatform.Data.Repositories
{
    public class ProjectMatrixRequirementsRepository : RepositoryBase<LmPlatformModelsContext, ProjectMatrixRequirement>, IProjectMatrixRequirementsRepository
    {

        public ProjectMatrixRequirementsRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {
        }

        public void DeleteAll(int projectId)
        {
            var records = DataContext.ProjectMatrixRequirements.Where(e => e.ProjectId == projectId).ToList();
            foreach(var record in records)
            {
                DataContext.ProjectMatrixRequirements.Remove(record);
            }
            DataContext.SaveChanges();
        }

        public void UpdateCoveredWhere(Query<ProjectMatrixRequirement> query)
        {
            var records = GetAll(query);
            foreach (var record in records)
            {
                record.Covered = true;
            }
            DataContext.SaveChanges();
        }
    }
}
