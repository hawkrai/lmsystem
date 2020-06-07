using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMPlatform.UI.Services.Parental
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using LMPlatform.UI.Services.Modules;
    using LMPlatform.UI.Services.Modules.Parental;
    using LMPlatform.UI.Services.Parental.Models;

    [ServiceContract]
    public interface IParentalService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetGroupSubjects/{groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        SubjectListResult GetGroupSubjects(string groupId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/LoadGroup?groupId={groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        ParentalResult LoadGroup(string groupId);

    }
}