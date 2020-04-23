using System.ServiceModel;
using System.ServiceModel.Web;
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

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Projects/{id}/Bugs", ResponseFormat = WebMessageFormat.Json)]
        ProjectCommentsResult GetProjectComments(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Projects/{id}/SaveFile", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ProjectFileResult SaveFile(string id, ProjectFileViewData projectFile);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Projects/{id}/Attachments", ResponseFormat = WebMessageFormat.Json)]
        ProjectFilesResult GetAttachments(string id);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/Projects/{id}/Attachments/{fileName}", ResponseFormat = WebMessageFormat.Json)]
        void DeleteAttachment(string id, string fileName);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Projects/{id}/GenerateMatrix", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void GenerateMatrix(string id, ProjectMatrixViewData projectMatrix);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Projects/{id}/Matrix", ResponseFormat = WebMessageFormat.Json)]
        ProjectMatrixResult GetMatrix(string id);
    }
}
