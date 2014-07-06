using System.Web.Http;
using System.Web.Http.ModelBinding;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.DTO;
using LMPlatform.UI.App_Start;

namespace LMPlatform.UI.ApiControllers.DP
{
    public class PercentageController : ApiController
    {
        private readonly LazyDependency<IPercentageGraphService> percentageService = new LazyDependency<IPercentageGraphService>();

        private IPercentageGraphService PercentageService
        {
            get { return percentageService.Value; }
        }

        public PagedList<PercentageGraphData> Get([ModelBinder]GetPagedListParams parms)
        {
            return PercentageService.GetPercentageGraphs(parms);
        }
    }
}
