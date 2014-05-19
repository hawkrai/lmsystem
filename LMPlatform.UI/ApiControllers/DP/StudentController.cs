using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.DPManagement;

namespace LMPlatform.UI.ApiControllers.DP
{
    public class StudentController : ApiController
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private readonly LazyDependency<IDpManagementService> dpManagementService = new LazyDependency<IDpManagementService>();

        private IDpManagementService DpManagementService
        {
            get { return dpManagementService.Value; }
        }

        public object Get()
        {
            var nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var count = int.Parse(nvc["count"]);
            var page = int.Parse(nvc["page"]);
            var diplomProjectId = int.Parse(nvc["diplomProjectId"]);

            int total;
            return new
            {
                Data = DpManagementService.GetStudentsByDiplomProjectId(diplomProjectId, page, count, out total),
                Total = total
            };
        }
    }
}