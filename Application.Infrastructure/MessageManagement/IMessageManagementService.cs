using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace Application.Infrastructure.MessageManagement
{
    public interface IMessageManagementService
    {
        IEnumerable<User> GetRecipients(int userId);

        IEnumerable<User> GetMessageRecipients(int messageId);

        Message SaveMessage(Message message);

        void SaveUserMessages(ICollection<UserMessages> userMessages);

        UserMessages SaveUserMessages(UserMessages userMessages);

        ////List<UserMessages> GetIncomingUserMessages(int userId);
        ////List<UserMessages> GetOutcomingUserMessages(int userId);

        IEnumerable<UserMessages> GetCorrespondence(int firstUserId, int secondUserId);

        IEnumerable<UserMessages> GetUnreadUserMessages(int userId);

        IEnumerable<UserMessages> GetUserMessages(int userId);

        IPageableList<UserMessages> GetUserMessagesPageable(int userId, bool? incoming = null, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        Message GetMessage(int messageId);

        UserMessages SetRead(int userMessageId);

        UserMessages GetUserMessage(int userMessageId);
    }
}
