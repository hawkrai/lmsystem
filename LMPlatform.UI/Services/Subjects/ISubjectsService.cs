using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.Parental;
using System.ServiceModel.Activation;

namespace LMPlatform.UI.Services.Subjects
{
    [ServiceContract]
    public interface ISubjectsService
    {
        [OperationContract]
        [WebInvoke(Method = "PATCH", UriTemplate = "/Subjects", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        SubjectResult Update(SubjectViewData subject);
    }
}
