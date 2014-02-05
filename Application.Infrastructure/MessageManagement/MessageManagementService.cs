using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Constants;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

namespace Application.Infrastructure.MessageManagement
{
    public class MessageManagementService : IMessageManagementService
    {
        public List<User> GetRecipientsList(int currentUserId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var currentUser = repositoriesContainer.UsersRepository
                    .GetBy(new Query<User>(u => u.Id == currentUserId).Include(u => u.Membership.Roles));

                return GetRecipientsList(currentUser);
            }
        }

        public List<User> GetRecipientsList()
        {
            throw new NotImplementedException();
        }

        public Message SaveMessage(Message message)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.MessageRepository.Save(message);
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

        public List<UserMessages> GetCorrespondence(int firstUserId, int secondUserId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var userMessages = repositoriesContainer.MessageRepository.GetCorrespondence(firstUserId, secondUserId);
                return userMessages;
            }
        }

        public List<UserMessages> GetUnreadUserMessages(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var userMessages = repositoriesContainer.MessageRepository.GetUserMessages(userId);
                return userMessages.Where(m => m.RecipientId == userId && !m.IsReaded).ToList();
            }
        }

        public List<UserMessages> GetUserMessages(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var userMessages = repositoriesContainer.MessageRepository.GetUserMessages(userId);
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

        private List<User> GetRecipientsList(User currentUser)
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
