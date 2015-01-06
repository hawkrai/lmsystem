using System.Net;
using System.Net.Http;
using System.Web.Http;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.DTO;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.DP
{
    public class PercentageResultController : ApiController
    {
        public object Get([System.Web.Http.ModelBinding.ModelBinder]GetPagedListParams parms)
        {
            return new
            {
                Students = DpManagementService.GetGraduateStudentsForUser(WebSecurity.CurrentUserId, parms),
                PercentageGraphs = PercentageService.GetPercentageGraphsForLecturerAll(WebSecurity.CurrentUserId, parms)
            };
        }

        public HttpResponseMessage Post([FromBody]PercentageResultData percentage)
        {
            return SavePercentageResult(percentage);
        }
        
        public HttpResponseMessage Put([FromBody]PercentageResultData percentage)
        {
            return SavePercentageResult(percentage);
        }

        private HttpResponseMessage SavePercentageResult(PercentageResultData percentageResult)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            PercentageService.SavePercentageResult(WebSecurity.CurrentUserId, percentageResult);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private IPercentageGraphService PercentageService
        {
            get { return _percentageService.Value; }
        }

        private IDpManagementService DpManagementService
        {
            get { return _diplomProjectManagementService.Value; }
        }

        private readonly LazyDependency<IDpManagementService> _diplomProjectManagementService = new LazyDependency<IDpManagementService>();

        private readonly LazyDependency<IPercentageGraphService> _percentageService = new LazyDependency<IPercentageGraphService>();
    }
}