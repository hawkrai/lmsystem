using System.Collections.Generic;
using Application.Core.Data;
using Application.Infrastructure.DTO;

namespace Application.Infrastructure.DPManagement
{
    public interface IPercentageGraphService
    {
        PagedList<PercentageGraphData> GetPercentageGraphs(GetPagedListParams parms);
    }
}
