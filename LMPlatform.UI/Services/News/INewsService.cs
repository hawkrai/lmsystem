using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LMPlatform.UI.Services.News
{
    using System.ServiceModel.Web;
    using System.Web.Mvc;

    using LMPlatform.Models;
    using LMPlatform.UI.Services.Modules;
    using LMPlatform.UI.Services.Modules.News;

    [ServiceContract]
    public interface INewsService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetNews/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        NewsResult GetNews(string subjectId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/DisableNews")]
		ResultViewData DisableNews(string subjectId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/EnableNews")]
		ResultViewData EnableNews(string subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Save")]
        ResultViewData Save(string subjectId, string id, string title, string body, bool disabled, bool isOldDate);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Delete")]
        ResultViewData Delete(string id, string subjectId);
    }
}
