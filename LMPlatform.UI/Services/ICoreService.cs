using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LMPlatform.UI.Services
{
    using System.ServiceModel.Web;

    using LMPlatform.UI.Services.Modules.CoreModels;
    using LMPlatform.UI.Services.Modules.Labs;

    [ServiceContract]
    public interface ICoreService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetGroups/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        GroupsResult GetGroups(string subjectId);
    }
}
