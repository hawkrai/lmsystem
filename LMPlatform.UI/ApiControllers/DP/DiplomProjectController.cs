using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
            
            int total;
            return new
                {
                    Data = DpManagementService.GetProjects(page, count, out total),
                    Total = total
                };
        }
        
        public object Get(int id)
        {
            var dp = DpManagementService.GetProject(id);
            var groups = DpManagementService.GetAllGroups();

            return new
                {
                    Project = dp,
                    Groups = groups.Select(g => new
                        {
                            Id = g.Id,
                            Name = g.Name
                        }),
                };
        }

        public void Post([FromBody]DiplomProjectData project)
        {
            project.LecturerId = WebSecurity.CurrentUserId;
            DpManagementService.SaveProject(project);
        }

        public void Put([FromBody]DiplomProjectData project)
        {
            project.LecturerId = WebSecurity.CurrentUserId;
            DpManagementService.SaveProject(project);
        }

        public void Delete(int id)
        {
        }
    }
}
