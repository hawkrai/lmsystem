using System.Net;
using System.Net.Http;
using System.Web.Http;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.CPManagement;
using Application.Infrastructure.CTO;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.CP
{
    public class CpPercentageResultController : ApiController
    {
        public object Get([System.Web.Http.ModelBinding.ModelBinder]GetPagedListParams parms)
        {
            var subjectId = 0;
            if (parms.Filters.ContainsKey("subjectId"))
            {
                subjectId = int.Parse(parms.Filters["subjectId"]);
            }

            var groupId = 0;
            if (parms.Filters.ContainsKey("groupId"))
            {
                groupId = int.Parse(parms.Filters["groupId"]);
            }

            return new
            {
                Students = CpManagementService.GetGraduateStudentsForGroup(WebSecurity.CurrentUserId, groupId, subjectId, parms, false),
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

        private ICpPercentageGraphService PercentageService
        {
            get { return _percentageService.Value; }
        }

        private ICPManagementService CpManagementService
        {
            get { return _courseProjectManagementService.Value; }
        }

        private readonly LazyDependency<ICPManagementService> _courseProjectManagementService = new LazyDependency<ICPManagementService>();

        private readonly LazyDependency<ICpPercentageGraphService> _percentageService = new LazyDependency<ICpPercentageGraphService>();
    }
}