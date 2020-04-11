using System.ServiceModel;
using System.ServiceModel.Web;
using LMPlatform.UI.Services.Modules.Files;

namespace LMPlatform.UI.Services.Files
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFilesService" in both code and config file together.
	[ServiceContract]
	public interface IFilesService
	{
		[OperationContract]
		[WebInvoke(UriTemplate = "/GetFiles/", RequestFormat = WebMessageFormat.Json, Method = "GET")]
		AttachmentResult GetFiles();

		[OperationContract]
		[WebInvoke(
			UriTemplate = "/GetFilesWithPagination?filter={namePart}&filesPerPage={filesPerPage}&page={pageNumber}",
			RequestFormat = WebMessageFormat.Json, Method = "GET")]
		AttachmentResult GetFilesWithPagination(string namePart, int filesPerPage, int pageNumber);
	}
}