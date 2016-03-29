using System.Web.Http;
using Application.Core;
using Application.Infrastructure.CPManagement;
using LMPlatform.Models.CP;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.CP
{
    public class CpTaskSheetTemplateController : ApiController
    {
        public CourseProjectTaskSheetTemplate Get(int templateId)
        {
            return CpManagementService.GetTaskSheetTemplate(templateId);
        }

        public void Post([FromBody] CourseProjectTaskSheetTemplate template)
        {
            template.LecturerId = WebSecurity.CurrentUserId;
            CpManagementService.SaveTaskSheetTemplate(template);
        }

        private ICPManagementService CpManagementService
        {
            get { return courseProjectManagementService.Value; }
        }

        private readonly LazyDependency<ICPManagementService> courseProjectManagementService = new LazyDependency<ICPManagementService>();
    }
}
