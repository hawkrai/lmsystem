using System.ServiceModel;
using System.ServiceModel.Web;
using LMPlatform.UI.Services.Modules.Parental;

namespace LMPlatform.UI.Services.Parental
{
	[ServiceContract]
	public interface IParentalService
	{
		[OperationContract]
		[WebInvoke(UriTemplate = "/GetGroupSubjects/{groupId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		SubjectListResult GetGroupSubjects(string groupId);
	}
}