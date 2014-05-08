using System.Collections.Generic;
using Application.Infrastructure.DTO;

namespace Application.Infrastructure.DPManagement
{
    public interface ICorrelationService
    {
        List<Correlation> GetCorrelation(string entity);
    }
}
