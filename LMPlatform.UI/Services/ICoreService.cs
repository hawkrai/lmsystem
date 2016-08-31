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

    [ServiceContract]
    public interface ICoreService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetGroups/{subjectId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        GroupsResult GetGroups(string subjectId);

		[OperationContract]
		[WebInvoke(UriTemplate = "/GetAllGroupsLite", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		GroupsResult GetAllGroupsLite();

	    [OperationContract]
	    [WebInvoke(UriTemplate = "/GetStudentsByGroupId/{groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
	    StudentsResult GetStudentsByGroupId(string groupId);

	    [OperationContract]
	    [WebInvoke(UriTemplate = "/СonfirmationStudent/{studentId}", RequestFormat = WebMessageFormat.Json, Method = "PUT")]
	    StudentsResult СonfirmationStudent(string studentId);

		[OperationContract]
		[WebInvoke(UriTemplate = "/GetSubjectsByOwnerUser/", RequestFormat = WebMessageFormat.Json, Method = "GET")]
	    SubjectResult GetSubjectsByOwnerUser();

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
    }
}
