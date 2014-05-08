using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.DTO;

namespace LMPlatform.UI.ApiControllers.DP
{
    public class CorrelationController : ApiController
    {
        private readonly LazyDependency<ICorrelationService> correlationService = new LazyDependency<ICorrelationService>();

        private ICorrelationService CorrelationService
        {
            get { return correlationService.Value; }
        }

        // GET api/<controller>
        public List<Correlation> Get()
        {
            var entity = HttpUtility.ParseQueryString(Request.RequestUri.Query)["entity"];
            return CorrelationService.GetCorrelation(entity);
        }
    }
}