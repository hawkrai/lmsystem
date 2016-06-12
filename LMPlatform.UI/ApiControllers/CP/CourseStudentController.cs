using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.CPManagement;
using Application.Infrastructure.CTO;

namespace LMPlatform.UI.ApiControllers.CP
{
    public class CourseStudentController : ApiController
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private readonly LazyDependency<ICPManagementService> cpManagementService = new LazyDependency<ICPManagementService>();

        private ICPManagementService CpManagementService
        {
            get { return cpManagementService.Value; }
        }

        public PagedList<StudentData> Get([ModelBinder]GetPagedListParams parms)
        {
            return CpManagementService.GetStudentsByCourseProjectId(parms);
        }
    }
}