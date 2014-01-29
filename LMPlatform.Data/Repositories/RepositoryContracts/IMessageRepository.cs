using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IMessageRepository : IRepositoryBase<Message>
    {
        UserMessages SaveUserMessages(UserMessages userMessages);

        List<UserMessages> GetUserMessages(int userId);

        List<UserMessages> GetCorrespondence(int firstUserId, int secondUserId);
    }
}
