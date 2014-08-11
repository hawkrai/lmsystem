using System.Net;
using System.Net.Http;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.DPManagement;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.DP
{
    public class StudentMarkController : ApiController
    {
        public HttpResponseMessage Post([FromBody]int[] mark)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            DpManagementService.SetStudentDiplomMark(WebSecurity.CurrentUserId, mark[0], mark[1]);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private IDpManagementService DpManagementService
        {
            get { return _diplomProjectManagementService.Value; }
        }

        private readonly LazyDependency<IDpManagementService> _diplomProjectManagementService = new LazyDependency<IDpManagementService>();
    }
}