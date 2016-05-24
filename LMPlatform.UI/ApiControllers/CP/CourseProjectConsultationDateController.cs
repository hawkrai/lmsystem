using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.CPManagement;
using WebMatrix.WebData;
using Application.Infrastructure.CTO;

namespace LMPlatform.UI.ApiControllers.CP
{
    public class CourseProjectConsultationDateController : ApiController
    {
        public HttpResponseMessage Post([FromBody]/*DateTime consultationDate, int subject*/CourseProjectConsultationDateData consultationDate)
        {
            PercentageService.SaveConsultationDate(WebSecurity.CurrentUserId, consultationDate.Day, consultationDate.SubjectId);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public void Delete(int id)
        {
            PercentageService.DeleteConsultationDate(WebSecurity.CurrentUserId, id);
        }

        private ICpPercentageGraphService PercentageService
        {
            get { return _percentageService.Value; }
        }

        private readonly LazyDependency<ICpPercentageGraphService> _percentageService = new LazyDependency<ICpPercentageGraphService>();
    }
}