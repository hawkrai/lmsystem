using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.DTO;

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

        public PagedList<StudentData> Get([ModelBinder]GetPagedListParams parms)
        {
            return DpManagementService.GetStudentsByDiplomProjectId(parms);
        }
    }
}