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

        public IEnumerable<UserMessages> GetUserMessages(int userId)
        {
            return DataContext.Set<UserMessages>()
                              .Include("Message")
                              .Include("Author.Lecturer")
                              .Include("Author.Student")
                              .Where(m => m.AuthorId == userId || m.RecipientId == userId);
        }

        public List<UserMessages> GetCorrespondence(int firstUserId, int secondUserId)
        {
            return DataContext.Set<UserMessages>()
                .Where(m => (m.AuthorId == firstUserId && m.RecipientId == secondUserId) ||
                            (m.AuthorId == secondUserId && m.RecipientId == firstUserId))
                .ToList();
        }
    }
}
