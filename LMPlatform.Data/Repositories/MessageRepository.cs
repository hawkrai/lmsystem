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

        public UserMessages GetUserMessage(int messageId, int userId)
        {
            return
                DataContext.Set<UserMessages>()
                    .Include("Message")
                    .Include("Message.Attachments")
                    .Include("Author.Lecturer")
                    .Include("Author.Student")
                    .First(m => m.MessageId == messageId 
                        && (m.AuthorId == userId || m.RecipientId == userId) 
                        && (m.DeletedById != userId));
        }

        public UserMessages GetUserMessage(int userMessagesId)
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
                    .Where(m => (m.AuthorId == userId || m.RecipientId == userId) && (m.DeletedById != userId));
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

        public bool DeleteUserMessages(int userId)
        {
            var targets = DataContext.Set<UserMessages>().Where(m => m.AuthorId == userId).ToArray();
            var context = DataContext.Set<UserMessages>();
            try
            {
                foreach (var item in targets)
                {
                    context.Remove(item);
                }

                DataContext.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool DeleteMessage(int messageId, int userId)
        {
            try
            {
                {
                    var context = DataContext.Set<UserMessages>();

                    var reciepMsg = context.FirstOrDefault(e => e.MessageId == messageId && e.RecipientId == userId);

                    if (reciepMsg != null)
                    {
                        if (reciepMsg.DeletedById == reciepMsg.AuthorId)
                        {
                            context.Remove(reciepMsg);
                        }
                        else
                        {
                            reciepMsg.DeletedById = userId;
                        }
                    }

                    var authorMsg = context.Where(e => e.MessageId == messageId && e.AuthorId == userId);

                    foreach (var msg in authorMsg)
                    {
                        if (msg.DeletedById == msg.RecipientId)
                        {
                            context.Remove(msg);
                        }
                        else
                        {
                            msg.DeletedById = userId;
                        }
                    }

                    DataContext.SaveChanges();

                    if (!context.Any(e => e.MessageId == messageId))
                    {
                        var msg = GetBy(new Query<Message>(m => m.Id == messageId).Include(m => m.Attachments));
                        if (msg.Attachments.Any())
                        {
                            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
                            {
                                repositoriesContainer.AttachmentRepository.Delete(msg.Attachments);
                                repositoriesContainer.MessageRepository.Delete(msg);
                                repositoriesContainer.ApplyChanges();
                            }
                        }
                    }
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
