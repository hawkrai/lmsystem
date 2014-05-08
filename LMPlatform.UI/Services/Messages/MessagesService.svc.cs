using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Infrastructure.MessageManagement;
using LMPlatform.Models;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.Messages;
using LMPlatform.UI.ViewModels.MessageViewModels;
using Newtonsoft.Json;

namespace LMPlatform.UI.Services.Messages
{
    public class MessagesService : IMessagesService
    {
        private readonly LazyDependency<IMessageManagementService> _messageManagementService =
            new LazyDependency<IMessageManagementService>();

        public IMessageManagementService MessageManagementService
        {
            get { return _messageManagementService.Value; }
        }

        public MessagesResult GetMessages(string userId)
        {
            try
            {
                var model =
                    MessageManagementService.GetUserMessages(int.Parse(userId)).Select(e => new MessagesViewData(e)).ToList();

                return new MessagesResult
                    {
                        Messages = model,
                        Message = "Сообщения успешно загружены",
                        Code = "200"
                    };
            }
            catch (Exception e)
            {
                return new MessagesResult
                    {
                        Message = "Произошла ошибка при получении сообщений",
                        Code = "500"
                    };
            }
        }
    }
}