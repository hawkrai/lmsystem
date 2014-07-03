using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Application.Core;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.DTO;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.DP
{
    public class DiplomProjectController : ApiController
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

            var sorting = "Theme";
            var sortingKey = nvc.AllKeys.FirstOrDefault(x => x.StartsWith("sorting["));
            if (sortingKey != null)
            {
                sorting = sortingKey.Replace("sorting[", string.Empty).Replace("]", string.Empty) + " " + nvc[sortingKey];
            }
            
            int total;
            return new
                {
                    Data = DpManagementService.GetProjects(page, count, sorting, out total),
                    Total = total
                };
        }
        
        public DiplomProjectData Get(int id)
        {
            return DpManagementService.GetProject(id);
        }

        public HttpResponseMessage Post([FromBody]DiplomProjectData project)
        {
            return SaveProject(project);
        }

        public HttpResponseMessage Put([FromBody]DiplomProjectData project)
        {
            return SaveProject(project);
        }

        public void Delete(int id)
        {
            DpManagementService.DeleteProject(WebSecurity.CurrentUserId, id);
        }

        private HttpResponseMessage SaveProject(DiplomProjectData project)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            project.LecturerId = WebSecurity.CurrentUserId;
            DpManagementService.SaveProject(project);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
