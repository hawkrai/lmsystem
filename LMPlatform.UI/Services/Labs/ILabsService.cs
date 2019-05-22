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
		[WebInvoke(UriTemplate = "/GetMarks?subjectId={subjectId}&groupId={groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		StudentsMarksResult GetMarks(int subjectId, int groupId);

		[OperationContract]
		[WebInvoke(UriTemplate = "/GetMarksV2?subjectId={subjectId}&groupId={groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		StudentsMarksResult GetMarksV2(int subjectId, int groupId);

		[OperationContract]
		[WebInvoke(UriTemplate = "/GetFilesV2?subjectId={subjectId}&groupId={groupId}&IsCp={isCp}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		StudentsMarksResult GetFilesV2(int subjectId, int groupId, bool isCp);
		
		[OperationContract]
		[WebInvoke(UriTemplate = "/GetLabsV2?subjectId={subjectId}&groupId={groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		LabsResult GetLabsV2(string subjectId, int groupId);

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
        ResultViewData SaveLabsVisitingData(int dateId, List<string> marks, List<string> comments, List<int> studentsId, List<int> Id, List<StudentsViewData> students);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveLabsVisitingDataSingle")]
		ResultViewData SaveLabsVisitingDataSingle(int dateId, string mark, string comment, int studentsId, int id);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveStudentLabsMark")]
        ResultViewData SaveStudentLabsMark(int studentId, int labId, string mark, string comment, string date, int id, List<StudentsViewData> students);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/DeleteVisitingDate")]
        ResultViewData DeleteVisitingDate(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetFilesLab")]
        UserLabFilesResult GetFilesLab(string userId, string subjectId, bool isCoursPrj);

	    [OperationContract]
	    [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SendFile")]
	    ResultViewData SendFile(string subjectId, string userId, string id, string comments, string pathFile, string attachments, bool isCp, bool isRet);

	    [OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/DeleteUserFile")]
	    ResultViewData DeleteUserFile(string id);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/ReceivedLabFile")]
		ResultViewData ReceivedLabFile(string userFileId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/CancelReceivedLabFile")]
		ResultViewData CancelReceivedLabFile(string userFileId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/CheckPlagiarism")]
		ResultViewData CheckPlagiarism(string userFileId, string subjectId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/CheckPlagiarismSubjects")]
		ResultPSubjectViewData CheckPlagiarismSubjects(string subjectId, string type, string threshold);
    }
}
