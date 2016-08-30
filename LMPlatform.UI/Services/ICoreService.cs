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

		[OperationContract]
		[WebInvoke(UriTemplate = "/GetAllGroupsLite", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		GroupsResult GetAllGroupsLite();

	    [OperationContract]
	    [WebInvoke(UriTemplate = "/GetStudentsByGroupId/{groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
	    StudentsResult GetStudentsByGroupId(string groupId);

	    [OperationContract]
	    [WebInvoke(UriTemplate = "/СonfirmationStudent/{studentId}", RequestFormat = WebMessageFormat.Json, Method = "PUT")]
	    StudentsResult СonfirmationStudent(string studentId);
    }
}
