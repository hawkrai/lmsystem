using System.Net;
using System.Net.Http;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.CPManagement;
using Application.Infrastructure.CTO;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.CP
{
    public class CpTaskSheetController : ApiController
    {
        public object Get(int courseProjectId)
        {
            return CpManagementService.GetTaskSheet(courseProjectId);
        }

        public HttpResponseMessage Post([FromBody]TaskSheetData taskSheet)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            CpManagementService.SaveTaskSheet(WebSecurity.CurrentUserId, taskSheet);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private ICPManagementService CpManagementService
        {
            get { return _courseProjectManagementService.Value; }
        }

        private readonly LazyDependency<ICPManagementService> _courseProjectManagementService = new LazyDependency<ICPManagementService>();
    }
}