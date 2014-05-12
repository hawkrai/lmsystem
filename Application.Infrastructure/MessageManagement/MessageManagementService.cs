using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Core.Constants;
using Application.Core.Data;
using Application.Infrastructure.FilesManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

namespace Application.Infrastructure.MessageManagement
{
    using System.Web.Mvc;

    public class MessageManagementService : IMessageManagementService
    {
        private readonly LazyDependency<IFilesManagementService> _filesManagementService =
            new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get { return _filesManagementService.Value; }
        }

        public IEnumerable<User> GetRecipients(int currentUserId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var currentUser = repositoriesContainer.UsersRepository
                    .GetBy(new Query<User>(u => u.Id == currentUserId).Include(u => u.Membership.Roles));

                return GetRecipientsList(currentUser);
            }
        }

        public IEnumerable<User> GetMessageRecipients(int messageId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var recipients = repositoriesContainer.MessageRepository.GetMessageRecipients(messageId);

                return recipients;
            }
        }

        public Message SaveMessage(Message message)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.MessageRepository.Save(message);

                if (message.Attachments != null)
                {
                    FilesManagementService.SaveFiles(message.Attachments, message.AttachmentsPath.ToString());

                    foreach (var attach in message.Attachments)
                    {
                        repositoriesContainer.AttachmentRepository.Save(attach);
                    }
                }

                repositoriesContainer.ApplyChanges();
            }

            return message;
        }

        public void SaveUserMessages(ICollection<UserMessages> userMessages)
        {
            foreach (var userMsg in userMessages)
            {
                SaveUserMessages(userMsg);
            }
        }

        public UserMessages SaveUserMessages(UserMessages userMessages)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.MessageRepository.SaveUserMessages(userMessages);
                repositoriesContainer.ApplyChanges();
            }

            return userMessages;
        }

        public List<UserMessages> GetIncomingUserMessages(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var userMessages = repositoriesContainer.MessageRepository.GetUserMessages(userId);
                return userMessages.Where(m => m.RecipientId == userId).ToList();
            }
        }

        public List<UserMessages> GetOutcomingUserMessages(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var userMessages = repositoriesContainer.MessageRepository.GetUserMessages(userId);
                return userMessages.Where(m => m.AuthorId == userId).ToList();
            }
        }

        public IEnumerable<UserMessages> GetCorrespondence(int firstUserId, int secondUserId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var userMessages = repositoriesContainer.MessageRepository.GetCorrespondence(firstUserId, secondUserId);
                return userMessages;
            }
        }

        public IEnumerable<UserMessages> GetUnreadUserMessages(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var userMessages = repositoriesContainer.MessageRepository.GetUserMessages(userId);
                return userMessages.Where(m => m.RecipientId == userId && !m.IsRead).ToList();
            }
        }

        public IEnumerable<UserMessages> GetUserMessages(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var userMessages = repositoriesContainer.MessageRepository.GetUserMessages(userId).ToList();
                return userMessages;
            }
        }

        public IPageableList<UserMessages> GetUserMessagesPageable(int userId, bool? incoming = null, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<UserMessages>(pageInfo);

            if (!incoming.HasValue)
            {
                query.AddFilterClause(e => e.AuthorId == userId || e.Recipient.Id == userId);
            }
            else
            {
                if (incoming.Value)
                {
                    query.AddFilterClause(e => e.Recipient.Id == userId);
                }
                else
                {
                    query.AddFilterClause(e => e.AuthorId == userId);
                }
            }

            query.Include(e => e.Message.Attachments).Include(e => e.Recipient)
              .Include(e => e.Author.Lecturer).Include(e => e.Author.Student);

            if (!string.IsNullOrEmpty(searchString))
            {
                query.AddFilterClause(
                    e => e.Message.Text.ToLower().StartsWith(searchString) || e.Message.Text.ToLower().Contains(searchString));
            }

            query.OrderBy(sortCriterias);
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var userMessages = repositoriesContainer.RepositoryFor<UserMessages>().GetPageableBy(query);
                return userMessages;
            }
        }

        public Message GetMessage(int messageId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var message =
                    repositoriesContainer.MessageRepository.GetBy(
                        new Query<Message>(m => m.Id == messageId).Include(m => m.Attachments));
                return message;
            }
        }

        public UserMessages SetRead(int userMessageId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var message = repositoriesContainer.MessageRepository.GetUserMessagesById(userMessageId);
                message.IsRead = true;
                repositoriesContainer.ApplyChanges();
                return message;
            }
        }

        public UserMessages GetUserMessage(int userMessageId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var message = repositoriesContainer.MessageRepository.GetUserMessagesById(userMessageId);
                return message;
            }
        }

        public bool DeleteUserMessages(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var result = repositoriesContainer.MessageRepository.DeleteUserMessages(userId);
                repositoriesContainer.ApplyChanges();
                return result;
            }
        }

        public bool DeleteMessage(int messageId, int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var result = repositoriesContainer.MessageRepository.DeleteMessage(messageId, userId);
                repositoriesContainer.ApplyChanges();
                return result;
            }
        }

        private static IEnumerable<User> GetRecipientsList(User currentUser)
        {
            var recipientsList = new List<User>();
            if (currentUser != null
                && currentUser.Membership != null
                && currentUser.Membership.Roles != null
                && currentUser.Membership.Roles.Any())
            {
                var role = currentUser.Membership.Roles.Select(r => r.RoleName).ToArray().First();

                using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
                {
                    switch (role)
                    {
                        case Constants.Roles.Student:
                            recipientsList = repositoriesContainer.UsersRepository
                            .GetAll(new Query<User>(u => u.Id != currentUser.Id && u.Lecturer != null)
                            .Include(u => u.Lecturer)).ToList();
                            break;
                        case Constants.Roles.Lector:
                            recipientsList = repositoriesContainer.UsersRepository
                            .GetAll(new Query<User>(u => u.Id != currentUser.Id && (u.Lecturer != null || u.Student != null))
                            .Include(u => u.Student).Include(u => u.Lecturer)).ToList();
                            break;
                        case Constants.Roles.Admin:
                            recipientsList = repositoriesContainer.UsersRepository
                            .GetAll(new Query<User>(u => u.Id != currentUser.Id)
                            .Include(u => u.Student).Include(u => u.Lecturer)).ToList();
                            break;
                    }
                }
            }

            return recipientsList;
        }
    }
}
