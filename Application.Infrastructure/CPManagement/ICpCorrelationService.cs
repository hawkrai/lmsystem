using System.Collections.Generic;
using Application.Infrastructure.CTO;

namespace Application.Infrastructure.CPManagement
{
    public interface ICpCorrelationService
    {
        List<Correlation> GetCorrelation(string entity, int subjectId, int? id);
    }
}
