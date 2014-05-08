using System.Collections.Generic;
using System.Data.Entity;
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

        public UserMessages GetUserMessagesById(int userMessagesId)
        {
            return
                DataContext.Set<UserMessages>()
                    .Include("Message")
                    .Include("Message.Attachments")
                    .Include("Author.Lecturer")
                    .Include("Author.Student")
                    .SingleOrDefault(m => m.Id == userMessagesId);
        }

        public IEnumerable<UserMessages> GetUserMessages(int userId)
        {
            return
                DataContext.Set<UserMessages>()
                    .Include("Message")
                    .Include("Author.Lecturer")
                    .Include("Author.Student")
                    .Include("Message.Attachments")
                    .Where(m => m.AuthorId == userId || m.RecipientId == userId);
        }

        public IEnumerable<User> GetMessageRecipients(int messageId)
        {
            return
                DataContext.Set<UserMessages>()
                    .Where(m => m.MessageId == messageId)
                    .Select(u => u.Recipient)
                    .Include("Lecturer")
                    .Include("Student")
                    .ToList();
        }

        public List<UserMessages> GetCorrespondence(int firstUserId, int secondUserId)
        {
            return
                DataContext.Set<UserMessages>()
                    .Where(
                        m =>
                        (m.AuthorId == firstUserId && m.RecipientId == secondUserId)
                        || (m.AuthorId == secondUserId && m.RecipientId == firstUserId))
                    .ToList();
        }

        public bool DeleteUserMessage(int userId)
        {
            var targets = GetUserMessages(userId);
            var context = DataContext.Set<UserMessages>();
            try
            {
                foreach (var item in targets)
                {
                    context.Remove(item);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
