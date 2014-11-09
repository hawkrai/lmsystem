using System.Web.Http;
using Application.Core;
using Application.Infrastructure.DPManagement;
using LMPlatform.Models.DP;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.DP
{
    public class TaskSheetTemplateController : ApiController
    {
        public DiplomProjectTaskSheetTemplate Get(int templateId)
        {
            return DpManagementService.GetTaskSheetTemplate(templateId);
        }

        public void Post([FromBody] DiplomProjectTaskSheetTemplate template)
        {
            template.LecturerId = WebSecurity.CurrentUserId;
            DpManagementService.SaveTaskSheetTemplate(template);
        }

        private IDpManagementService DpManagementService
        {
            get { return diplomProjectManagementService.Value; }
        }

        private readonly LazyDependency<IDpManagementService> diplomProjectManagementService = new LazyDependency<IDpManagementService>();
    }
}
