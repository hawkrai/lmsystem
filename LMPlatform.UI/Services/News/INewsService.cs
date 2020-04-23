using System.ServiceModel;
using System.ServiceModel.Web;
using LMPlatform.UI.Services.Modules.News;

namespace LMPlatform.UI.Services.News
{
	using Modules;
 
    [ServiceContract]
    public interface INewsService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetNews/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        NewsResult GetNews(string subjectId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/DisableNews")]
		ResultViewData DisableNews(int subjectId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/EnableNews")]
		ResultViewData EnableNews(int subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Save")]
        ResultViewData Save(int subjectId, int id, string title, string body, bool disabled, bool isOldDate);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Delete")]
        ResultViewData Delete(int id, int subjectId);
    }
}
