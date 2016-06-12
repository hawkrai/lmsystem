using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.CPManagement;
using Application.Infrastructure.CTO;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.CP
{
    public class CpCorrelationController : ApiController
    {
        private readonly LazyDependency<ICpCorrelationService> correlationService = new LazyDependency<ICpCorrelationService>();

        private ICpCorrelationService CorrelationService
        {
            get { return correlationService.Value; }
        }

        // GET api/<controller>
        public List<Correlation> Get()
        {
            var entity = HttpUtility.ParseQueryString(Request.RequestUri.Query)["entity"];
          
            var subjectId = HttpUtility.ParseQueryString(Request.RequestUri.Query)["subjectId"];
            if (subjectId == null)
            {
                return CorrelationService.GetCorrelation(entity, 0, WebSecurity.CurrentUserId);
            }
                return CorrelationService.GetCorrelation(entity, int.Parse(subjectId), WebSecurity.CurrentUserId);
        }
    }
}