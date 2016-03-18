using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace LMPlatform.UI.Services.Search
{    
    [ServiceContract]
    public interface ISearchService
    {
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Message SearchStudents(string text);

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Message SearchProjects(string text);

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Message SearchGroups(string text);

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Message SearchLecturers(string text);
    }
}
