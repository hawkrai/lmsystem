using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LMPlatform.UI.Services.Modules.BTS;

namespace LMPlatform.UI.Services.BTS
{
    [ServiceContract]
    public interface IProjectsService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Index?pageSize={pageSize}&pageNumber={pageNumber}", ResponseFormat = WebMessageFormat.Json)]
        ProjectsResult Index(int pageSize, int pageNumber = 0);
    }
}
