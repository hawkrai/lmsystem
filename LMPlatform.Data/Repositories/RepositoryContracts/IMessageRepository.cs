using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IMessageRepository : IRepositoryBase<Message>
    {
        UserMessages SaveUserMessages(UserMessages userMessages);

        UserMessages GetUserMessagesById(int userMessagesId);

        IEnumerable<UserMessages> GetUserMessages(int userId);

        IEnumerable<User> GetMessageRecipients(int messageId);

        List<UserMessages> GetCorrespondence(int firstUserId, int secondUserId);
    }
}
