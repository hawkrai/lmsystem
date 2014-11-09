using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.DTO;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.DP
{
    public class PercentageController : ApiController
    {
        private readonly LazyDependency<IPercentageGraphService> _percentageService = new LazyDependency<IPercentageGraphService>();

        private IPercentageGraphService PercentageService
        {
            get { return _percentageService.Value; }
        }

        public PagedList<PercentageGraphData> Get([ModelBinder]GetPagedListParams parms)
        {
            return PercentageService.GetPercentageGraphs(WebSecurity.CurrentUserId, parms);
        }

        public PercentageGraphData Get(int id)
        {
            return PercentageService.GetPercentageGraph(id);
        }

        public HttpResponseMessage Post([FromBody]PercentageGraphData percentage)
        {
            return SavePercentage(percentage);
        }

        public HttpResponseMessage Put([FromBody]PercentageGraphData percentage)
        {
            return SavePercentage(percentage);
        }

        public void Delete(int id)
        {
            PercentageService.DeletePercentage(WebSecurity.CurrentUserId, id);
        }

        private HttpResponseMessage SavePercentage(PercentageGraphData percentage)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            
            PercentageService.SavePercentage(WebSecurity.CurrentUserId, percentage);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
