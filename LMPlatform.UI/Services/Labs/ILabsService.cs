using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.Labs;

namespace LMPlatform.UI.Services.Labs
{
    using Modules.CoreModels;

    [ServiceContract]
    public interface ILabsService
    {
        // OK
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetLabs/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        LabsResult GetLabs(string subjectId);

        // OK
		[OperationContract]
		[WebInvoke(UriTemplate = "/GetMarks?subjectId={subjectId}&groupId={groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		StudentsMarksResult GetMarks(int subjectId, int groupId);

		// OK
		[OperationContract]
		[WebInvoke(UriTemplate = "/GetMarksV2?subjectId={subjectId}&groupId={groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		StudentsMarksResult GetMarksV2(int subjectId, int groupId);

		// OK
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetFilesV2?subjectId={subjectId}&groupId={groupId}&IsCp={isCp}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        StudentsMarksResult GetFilesV2(int subjectId, int groupId, bool isCp);

        // OK
        [OperationContract]
		[WebInvoke(UriTemplate = "/GetLabsV2?subjectId={subjectId}&groupId={groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		LabsResult GetLabsV2(int subjectId, int groupId);

		// OK
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Save")]
        ResultViewData Save(int subjectId, int id, string theme, int duration, int order, string shortName, string pathFile, string attachments);

        // OK
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Delete")]
        ResultViewData Delete(int id, int subjectId);

        // OK
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveScheduleProtectionDate")]
        ResultViewData SaveScheduleProtectionDate(int subGroupId, string date);

        // OK
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveLabsVisitingData")]
        ResultViewData SaveLabsVisitingData(int dateId, List<string> marks, List<string> comments, List<int> studentsId, List<int> Id, List<StudentsViewData> students);

        // OK
        [OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveLabsVisitingDataSingle")]
		ResultViewData SaveLabsVisitingDataSingle(int dateId, string mark, string comment, int studentsId, int id);

		// OK
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveStudentLabsMark")]
        ResultViewData SaveStudentLabsMark(int studentId, int labId, string mark, string comment, string date, int id, List<StudentsViewData> students);

        // OK
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/DeleteVisitingDate")]
        ResultViewData DeleteVisitingDate(int id);

        // OK
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetFilesLab")]
        UserLabFilesResult GetFilesLab(int userId, int subjectId, bool isCoursPrj);

        // OK
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SendFile")]
        ResultViewData SendFile(int subjectId, int userId, int id, string comments, string pathFile, string attachments, bool isCp, bool isRet);

        // OK
        [OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/DeleteUserFile")]
	    ResultViewData DeleteUserFile(int id);

		// OK
		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/ReceivedLabFile")]
		ResultViewData ReceivedLabFile(int userFileId);

		// OK
        [OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/CancelReceivedLabFile")]
		ResultViewData CancelReceivedLabFile(int userFileId);

		// OK
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/CheckPlagiarism")]
        ResultViewData CheckPlagiarism(int userFileId, int subjectId, bool isCp);

        // OK
        [OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/CheckPlagiarismSubjects")]
		ResultPSubjectViewData CheckPlagiarismSubjects(int subjectId, int type, int threshold);
	}
}
