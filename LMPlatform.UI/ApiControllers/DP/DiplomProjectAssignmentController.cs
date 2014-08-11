using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.DPManagement;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.DP
{
    public class DiplomProjectAssignmentController : ApiController
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private readonly LazyDependency<IDpManagementService> _dpManagementService = new LazyDependency<IDpManagementService>();

        private IDpManagementService DpManagementService
        {
            get { return _dpManagementService.Value; }
        }

        public void Post([FromBody]AssignProjectUpdateModel updateModel)
        {
            DpManagementService.AssignProject(WebSecurity.CurrentUserId, updateModel.ProjectId, updateModel.StudentId);
        }

        public void Delete(int id)
        {
            DpManagementService.DeleteAssignment(WebSecurity.CurrentUserId, id);
        }

        public class AssignProjectUpdateModel
        {
            public int ProjectId { get; set; } 

            public int StudentId { get; set; } 
        }
    }
}