using System.ServiceModel;
using System.ServiceModel.Web;
using LMPlatform.UI.Services.Modules.Messages;

namespace LMPlatform.UI.Services.Messages
{
	using Modules;

    [ServiceContract]
    public interface IMessagesService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetMessages/", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        MessagesResult GetMessages();

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetMessages?userId={userId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        MessagesResult GetMessagesByUserId(int userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetMessage/{id}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        DisplayMessageResult GetMessage(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetMessage/{id}?userId={userId}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        DisplayMessageResult GetUserMessage(string id, int userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetRecipients/", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        RecipientsResult GetRecipients();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Save")]
        ResultViewData Save(string subject, string body, string recipients, string attachments);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SaveFrom")]
        ResultViewData SaveFromUserId(string subject, string body, string recipients, string attachments, int fromId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Delete")]
        ResultViewData Delete(int messageId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/DeleteUserMessage")]
        ResultViewData DeleteUserMessage(int messageId, int userId);
    }
}