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
    public class DipomProjectConsultationController : ApiController
    {
        public object Get([System.Web.Http.ModelBinding.ModelBinder]GetPagedListParams parms)
        {
            return new
            {
                Students = DpManagementService.GetGraduateStudentsForLecturer(WebSecurity.CurrentUserId, parms),
                DipomProjectConsultationDates = PercentageService.GetConsultationDatesForLecturer(WebSecurity.CurrentUserId)
            };
        }

        public HttpResponseMessage Post([FromBody]DipomProjectConsultationMarkData consultationMark)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            PercentageService.SaveConsultationMark(WebSecurity.CurrentUserId, consultationMark);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private IDpManagementService DpManagementService
        {
            get { return _diplomProjectManagementService.Value; }
        }

        private IPercentageGraphService PercentageService
        {
            get { return _percentageService.Value; }
        }

        private readonly LazyDependency<IDpManagementService> _diplomProjectManagementService = new LazyDependency<IDpManagementService>();

        private readonly LazyDependency<IPercentageGraphService> _percentageService = new LazyDependency<IPercentageGraphService>();
    }
}