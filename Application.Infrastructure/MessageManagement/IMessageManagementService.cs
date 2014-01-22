using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMPlatform.Models;

namespace Application.Infrastructure.MessageManagement
{
    public interface IMessageManagementService
    {
        List<User> GetRecipientsList(int currentUserId);

        List<User> GetRecipientsList();
    }
}
