using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LMPlatform.UI.Services
{
    using System.ServiceModel.Web;

    using LMPlatform.UI.Services.Modules;
    using LMPlatform.UI.Services.Modules.CoreModels;
    using LMPlatform.UI.Services.Modules.Labs;
    using LMPlatform.UI.Services.Modules.Lectures;

	[ServiceContract]
    public interface ICoreService
    {
        [OperationContract]
		[WebInvoke(UriTemplate = "/GetGroups?subjectId={subjectId}&groupId={groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        GroupsResult GetGroups(string subjectId, string groupId);

		[OperationContract]
        [WebInvoke(UriTemplate = "/GetOnlyGroups/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        GroupsResult GetOnlyGroups(string subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetAllGroupsLite/", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        GroupsResult GetAllGroupsLite();

		[WebInvoke(UriTemplate = "/GetGroupsV2/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		GroupsResult GetGroupsV2(string subjectId);

		[OperationContract]
		[WebInvoke(UriTemplate = "/GetLecturesMarkVisitingV2?subjectId={subjectId}&groupId={groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		LecturesMarkVisitingResult GetLecturesMarkVisitingV2(int subjectId, int groupId);

	    [OperationContract]
	    [WebInvoke(UriTemplate = "/GetStudentsByGroupId/{groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
	    StudentsResult GetStudentsByGroupId(string groupId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetStudentsByStudentGroupId/{subjectId}/{groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        StudentsResult GetStudentsByStudentGroupId(string groupId, string subjectId);

        [OperationContract]
	    [WebInvoke(UriTemplate = "/СonfirmationStudent/{studentId}", RequestFormat = WebMessageFormat.Json, Method = "PUT")]
	    StudentsResult СonfirmationStudent(string studentId);

		[OperationContract]
		[WebInvoke(UriTemplate = "/UnConfirmationStudent/{studentId}", RequestFormat = WebMessageFormat.Json, Method = "PUT")]
		StudentsResult UnConfirmationStudent(string studentId);

		[OperationContract]
		[WebInvoke(UriTemplate = "/GetSubjectsByOwnerUser/", RequestFormat = WebMessageFormat.Json, Method = "GET")]
	    SubjectsResult GetSubjectsByOwnerUser();

		[OperationContract]
		[WebInvoke(UriTemplate = "/GetNoAdjointLectors/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		LectorResult GetNoAdjointLectors(string subjectId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/JoinLector")]
	    ResultViewData JoinLector(string subjectId, string lectorId);

		[OperationContract]
		[WebInvoke(UriTemplate = "/GetJoinedLector/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
	    LectorResult GetJoinedLector(string subjectId);

	    [OperationContract]
		[WebInvoke(UriTemplate = "/DisjoinLector/", RequestFormat = WebMessageFormat.Json, Method = "POST")]
	    ResultViewData DisjoinLector(string subjectId, string lectorId);
        
        //TODO: Find way to reemove /All
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetLecturers/All", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        LectorResult GetLecturers();

		[OperationContract]
		[WebInvoke(UriTemplate = "/GetGroupsByUser/{userId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		GroupsResult GetGroupsByUser(string userId);
    }
}
