using System.Net;
using System.Net.Http;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.CPManagement;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.CP
{
    public class CourseStudentMarkController : ApiController
    {
        public HttpResponseMessage Post([FromBody]int[] mark)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            CpManagementService.SetStudentDiplomMark(WebSecurity.CurrentUserId, mark[0], mark[1]);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private ICPManagementService CpManagementService
        {
            get { return _courseProjectManagementService.Value; }
        }

        private readonly LazyDependency<ICPManagementService> _courseProjectManagementService = new LazyDependency<ICPManagementService>();
    }
}