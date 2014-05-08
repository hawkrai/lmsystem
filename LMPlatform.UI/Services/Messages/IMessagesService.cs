using System.ServiceModel;
using System.ServiceModel.Web;
using LMPlatform.UI.Services.Modules.Messages;

namespace LMPlatform.UI.Services.Messages
{
    [ServiceContract]
    public interface IMessagesService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetMessages/{userId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        MessagesResult GetMessages(string userId);
    }
}