using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class MessageRepository : RepositoryBase<LmPlatformModelsContext, Message>, IMessageRepository
    {
        public MessageRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {
        }

        public UserMessages SaveUserMessages(UserMessages userMessages)
        {
            DataContext.Set<UserMessages>().Add(userMessages);
            return userMessages;
        }

        public List<UserMessages> GetUserMessages(int userId)
        {
            return DataContext.Set<UserMessages>()
                .Where(m => m.AuthorId == userId || m.RecipientId == userId)
                .OrderBy(m => m.Date).ToList();
        }

        public List<UserMessages> GetCorrespondence(int firstUserId, int secondUserId)
        {
            return DataContext.Set<UserMessages>()
                .Where(m => (m.AuthorId == firstUserId && m.RecipientId == secondUserId) ||
                            (m.AuthorId == secondUserId && m.RecipientId == firstUserId))
                .OrderBy(m => m.Date).ToList();
        }
    }
}
