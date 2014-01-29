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
        List<User> GetRecipientsList(int userId);

        List<User> GetRecipientsList();

        Message SaveMessage(Message message);

        void SaveUserMessages(ICollection<UserMessages> userMessages);

        UserMessages SaveUserMessages(UserMessages userMessages);

        List<UserMessages> GetIncomingUserMessages(int userId);

        List<UserMessages> GetOutcomingUserMessages(int userId);

        List<UserMessages> GetCorrespondence(int firstUserId, int secondUserId);

        List<UserMessages> GetUnreadUserMessages(int userId);

        Message GetMessage(int messageId);
    }
}
