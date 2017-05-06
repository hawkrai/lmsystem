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
        [WebInvoke(Method = "GET", UriTemplate = "/Index?pageSize={pageSize}&pageNumber={pageNumber}&sortingPropertyName={sortingPropertyName}&desc={desc}&searchString={searchString}", ResponseFormat = WebMessageFormat.Json)]
        ProjectsResult Index(int pageSize, int pageNumber, string sortingPropertyName, bool desc = false, string searchString = null);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Show/{id}?withDetails={withDetails}", ResponseFormat = WebMessageFormat.Json)]
        ProjectResult Show(string id, bool withDetails);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/ProjectParticipationsByUser/{id}?pageSize={pageSize}&pageNumber={pageNumber}&sortingPropertyName={sortingPropertyName}&desc={desc}&searchString={searchString}", ResponseFormat = WebMessageFormat.Json)]
        UserProjectParticipationsResult ProjectParticipationsByUser(string id, int pageSize, int pageNumber, string sortingPropertyName, bool desc = false, string searchString = null);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/StudentsParticipationsByGroup/{id}?pageSize={pageSize}&pageNumber={pageNumber}", ResponseFormat = WebMessageFormat.Json)]
        StudentsParticipationsResult StudentsParticipationsByGroup(string id, int pageSize, int pageNumber);
    }
}
