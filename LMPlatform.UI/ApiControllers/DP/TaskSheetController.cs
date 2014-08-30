using System.Net;
using System.Net.Http;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.DTO;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.DP
{
    public class TaskSheetController : ApiController
    {
        public object Get(int diplomProjectId)
        {
            return DpManagementService.GetTaskSheet(diplomProjectId);
        }

        public HttpResponseMessage Post([FromBody]TaskSheetData taskSheet)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            DpManagementService.SaveTaskSheet(WebSecurity.CurrentUserId, taskSheet);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private IDpManagementService DpManagementService
        {
            get { return _diplomProjectManagementService.Value; }
        }

        private readonly LazyDependency<IDpManagementService> _diplomProjectManagementService = new LazyDependency<IDpManagementService>();
    }
}