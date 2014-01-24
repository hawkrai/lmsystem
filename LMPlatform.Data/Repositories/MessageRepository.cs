using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
