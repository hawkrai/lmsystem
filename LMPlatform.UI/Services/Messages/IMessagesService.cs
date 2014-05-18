using System.ServiceModel;
using System.ServiceModel.Web;
using LMPlatform.UI.Services.Modules.Messages;

namespace LMPlatform.UI.Services.Messages
{
    using System.Collections;
    using System.Collections.Generic;

    using LMPlatform.UI.Services.Modules;

    [ServiceContract]
    public interface IMessagesService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetMessages/", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        MessagesResult GetMessages();

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetMessage/{id}", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        DisplayMessageResult GetMessage(string id);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetRecipients/", RequestFormat = WebMessageFormat.Json, Method = "GET")]
        RecipientsResult GetRecipients();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Save")]
        ResultViewData Save(string subject, string body, string recipients, string attachments);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Delete")]
        ResultViewData Delete(int messageId);
    }
}