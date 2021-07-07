using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace LMPlatform.UI.Services.Search
{    
    [ServiceContract]
    public interface ISearchService
    {
        [OperationContract]
        [WebGet]
        Stream SearchStudents(string text);

        [OperationContract]
        [WebGet]
        Stream SearchProjects(string text);

        [OperationContract]
        [WebGet]
        Stream SearchGroups(string text);

        [OperationContract]
        [WebGet]
        Stream SearchLecturers(string text);
    }
}