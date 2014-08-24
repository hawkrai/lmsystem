using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.DPManagement;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.DP
{
    public class DiplomProjectConsultationDateController : ApiController
    {
        public HttpResponseMessage Post([FromBody]DateTime consultationDate)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            PercentageService.SaveConsultationDate(WebSecurity.CurrentUserId, consultationDate);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public void Delete(int id)
        {
            PercentageService.DeleteConsultationDate(WebSecurity.CurrentUserId, id);
        }

        private IPercentageGraphService PercentageService
        {
            get { return _percentageService.Value; }
        }

        private readonly LazyDependency<IPercentageGraphService> _percentageService = new LazyDependency<IPercentageGraphService>();
    }
}