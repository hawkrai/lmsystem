using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Infrastructure.MessageManagement;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.MessageViewModels
{
    public class MessageListViewModel
    {
        private readonly LazyDependency<IMessageManagementService> _messageManagementService =
            new LazyDependency<IMessageManagementService>();

        public IMessageManagementService MessageManagementService
        {
            get { return _messageManagementService.Value; }
        }

        public MessageListViewModel()
        {
            _messages = new List<DisplayMessageViewModel>(GetUserMessages());
        }

        private readonly List<DisplayMessageViewModel> _messages;

        private int UserId
        {
            get { return WebSecurity.CurrentUserId; }
        }

        public List<DisplayMessageViewModel> IncomingMessages
        {
            get { return _messages.Where(m => m.AuthorId != UserId).OrderByDescending(m => m.Date).ToList(); }
        }

        public List<DisplayMessageViewModel> OutcomingMessages
        {
            get { return _messages.Where(m => m.AuthorId == UserId).OrderByDescending(m => m.Date).ToList(); }
        }

        public List<DisplayMessageViewModel> UnreadMessages
        {
            get { return _messages.Where(m => m.AuthorId != UserId && !m.IsRead).OrderByDescending(m => m.Date).ToList(); }
        }

        private IEnumerable<DisplayMessageViewModel> GetUserMessages()
        {
            var userMessages = MessageManagementService.GetUserMessages(UserId).Select(m => new DisplayMessageViewModel(m)).ToList();

            return userMessages;
        }
    }
}