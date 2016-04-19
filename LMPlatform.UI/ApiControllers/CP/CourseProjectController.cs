using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.CPManagement;
using Application.Infrastructure.CTO;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.CP
{
    public class CourseProjectController : ApiController
    {

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private readonly LazyDependency<ICPManagementService> _cpManagementService = new LazyDependency<ICPManagementService>();

        private ICPManagementService CpManagementService
        {
            get { return _cpManagementService.Value; }
        }

        public PagedList<CourseProjectData> Get([ModelBinder]GetPagedListParams parms)
        {
            return CpManagementService.GetProjects(WebSecurity.CurrentUserId, parms);
        }

        public CourseProjectData Get(int id)
        {
            return CpManagementService.GetProject(id);
        }

        public HttpResponseMessage Post([FromBody]CourseProjectData project)
        {
            return SaveProject(project);
        }

        public HttpResponseMessage Put([FromBody]CourseProjectData project)
        {
            return SaveProject(project);
        }

        public void Delete(int id)
        {
            CpManagementService.DeleteProject(WebSecurity.CurrentUserId, id);
        }

        private HttpResponseMessage SaveProject(CourseProjectData project)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            project.LecturerId = WebSecurity.CurrentUserId;
            
            CpManagementService.SaveProject(project);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
