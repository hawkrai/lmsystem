using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IMessageRepository : IRepositoryBase<Message>
    {
        UserMessages SaveUserMessages(UserMessages userMessages);

        UserMessages GetUserMessage(int userMessagesId);

        UserMessages GetUserMessage(int messageId, int userId);

        IEnumerable<UserMessages> GetUserMessages(int userId);

        IEnumerable<User> GetMessageRecipients(int messageId);

        List<UserMessages> GetCorrespondence(int firstUserId, int secondUserId);

        bool DeleteUserMessages(int userId);

        bool DeleteMessage(int messageId, int userId);
    }
}
