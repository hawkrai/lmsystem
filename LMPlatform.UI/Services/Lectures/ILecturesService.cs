using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LMPlatform.UI.Services.Lectures
{
    using System.ServiceModel.Web;

    using LMPlatform.UI.Services.Modules;
    using LMPlatform.UI.Services.Modules.Lectures;
    using LMPlatform.UI.Services.Modules.News;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILecturesService" in both code and config file together.
    [ServiceContract]
    public interface ILecturesService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetLectures/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        LecturesResult GetLectures(string subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Save")]
        ResultViewData Save(string subjectId, string id, string theme, string duration, string pathFile, string attachments);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Delete")]
        ResultViewData Delete(string id, string subjectId);
    }
}
