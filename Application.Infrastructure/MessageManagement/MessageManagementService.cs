using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<User> GetRecipientsList(User currentUser)
        {
            var userRoles = currentUser.Membership.Roles.Select(r => r.RoleName).ToArray();
            var recipientsList = new List<User>();
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                if (userRoles.Contains("admin"))
                {
                    recipientsList = repositoriesContainer.UsersRepository.GetAll(new Query<User>(u => u.Id != currentUser.Id)).ToList();
                }
            }

            return recipientsList;
        }
    }
}
