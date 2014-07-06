using System.Linq;
using Application.Core;
using Application.Core.Data;
using Application.Core.Extensions;
using Application.Infrastructure.DTO;
using LMPlatform.Data.Infrastructure;

namespace Application.Infrastructure.DPManagement
{
    public class PercentageGraphService : IPercentageGraphService
    {
        private readonly LazyDependency<IDpContext> context = new LazyDependency<IDpContext>();

        private IDpContext Context
        {
            get { return context.Value; }
        }

        public PagedList<PercentageGraphData> GetPercentageGraphs(GetPagedListParams parms)
        {
            return Context.DiplomPercentagesGraphs
                .AsNoTracking()
                .Select(x => new PercentageGraphData
                {
                    Id = x.Id,
                    Date = x.Date,
                    Name = x.Name,
                    Percentage = x.Percentage
                })
                .ApplyPaging(parms);
        }
    }
}