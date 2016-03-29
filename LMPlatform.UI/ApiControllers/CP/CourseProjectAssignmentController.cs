using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.CPManagement;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.CP
{
    public class CourseProjectAssignmentController : ApiController
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private readonly LazyDependency<ICPManagementService> _cpManagementService = new LazyDependency<ICPManagementService>();

        private ICPManagementService CpManagementService
        {
            get { return _cpManagementService.Value; }
        }

        public void Post([FromBody]AssignProjectUpdateModel updateModel)
        {
            CpManagementService.AssignProject(WebSecurity.CurrentUserId, updateModel.ProjectId, updateModel.StudentId);
        }

        public void Delete(int id)
        {
            CpManagementService.DeleteAssignment(WebSecurity.CurrentUserId, id);
        }

        public class AssignProjectUpdateModel
        {
            public int ProjectId { get; set; }

            public int StudentId { get; set; }
        }
    }
}
