using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.Labs;
using LMPlatform.UI.Services.Modules.Lectures;

namespace LMPlatform.UI.Services.Labs
{
    using LMPlatform.UI.Services.Modules.CoreModels;

    [ServiceContract]
    public interface ILabsService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetLabs/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        LabsResult GetLabs(string subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Save")]
        ResultViewData Save(string subjectId, string id, string theme, string duration, string order, string shortName, string pathFile, string attachments);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Delete")]
        ResultViewData Delete(string id, string subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveScheduleProtectionDate")]
        ResultViewData SaveScheduleProtectionDate(string subGroupId, string date);
        
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveLabsVisitingData")]
        ResultViewData SaveLabsVisitingData(List<StudentsViewData> students);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveStudentLabsMark")]
        ResultViewData SaveStudentLabsMark(List<StudentsViewData> students);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/DeleteVisitingDate")]
        ResultViewData DeleteVisitingDate(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetFilesLab")]
        UserLabFilesResult GetFilesLab(string userId, string subjectId);

	    [OperationContract]
	    [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SendFile")]
	    ResultViewData SendFile(string subjectId, string userId, string id, string comments, string pathFile, string attachments);
    }
}
