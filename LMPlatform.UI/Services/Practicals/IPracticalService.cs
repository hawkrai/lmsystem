using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.CoreModels;
using LMPlatform.UI.Services.Modules.Labs;
using LMPlatform.UI.Services.Modules.Practicals;

namespace LMPlatform.UI.Services.Practicals
{
    [ServiceContract]
    public interface IPracticalService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetPracticals/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        PracticalsResult GetLabs(string subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Save")]
        ResultViewData Save(string subjectId, string id, string theme, string duration, string order, string shortName, string pathFile, string attachments);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Delete")]
        ResultViewData Delete(string id, string subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveScheduleProtectionDate")]
        ResultViewData SaveScheduleProtectionDate(string groupId, string date, string subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetPracticalsVisitingData")]
        List<PracticalVisitingMarkViewData> GetPracticalsVisitingData(string dateId, string subGroupId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SavePracticalsVisitingData")]
        ResultViewData SavePracticalsVisitingData(List<PracticalVisitingMarkViewData> marks);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveStudentPracticalsMark")]
        ResultViewData SaveStudentPracticalsMark(StudentsViewData student);
    }
}
