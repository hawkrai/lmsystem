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
    public interface IBugsService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Index?pageSize={pageSize}&pageNumber={pageNumber}&sortingPropertyName={sortingPropertyName}&desc={desc}&searchString={searchString}", ResponseFormat = WebMessageFormat.Json)]
        BugsResult Index(int pageSize, int pageNumber, string sortingPropertyName, bool desc = false, string searchString = null);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Projects/{projectId}/Index?pageSize={pageSize}&pageNumber={pageNumber}&sortingPropertyName={sortingPropertyName}&desc={desc}&searchString={searchString}", ResponseFormat = WebMessageFormat.Json)]
        BugsResult ProjectBugs(string projectId, int pageSize, int pageNumber, string sortingPropertyName, bool desc = false, string searchString = null);
    }
}
