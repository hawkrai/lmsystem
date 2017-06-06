using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using LMPlatform.Models;
using Microsoft.AspNet.SignalR;
using LMPlatform.UI;
using Microsoft.AspNet.SignalR.Hubs;
using WebMatrix.WebData;

namespace LMPlatform.UI
{
     [HubName("chatHub")]
    public class ChatHub : Hub
    {
        #region Data Members

        static readonly List<UserDetail> ConnectedUsers = new List<UserDetail>();
        static readonly List<MessageDetail> CurrentMessage = new List<MessageDetail>();

        #endregion

        #region Methods

        public void Connect(string userName, string userRole)
        {
            var id = Context.ConnectionId;
            userName = WebSecurity.CurrentUserName;
            //if (Roles.IsUserInRole(userName, "admin")) userRole = "admin";
            //else if (Roles.IsUserInRole(userName, "lector")) userRole = "lector";
            //else userRole = "student";
            
            if (ConnectedUsers.Count(x => x.ConnectionId == id || x.UserName == userName || x.UserRole == userRole) == 0)
            {
                ConnectedUsers.Add(new UserDetail { ConnectionId = id, UserName = userName, UserRole = userRole});

                // send to caller
                Clients.Caller.onConnected(id, userName, ConnectedUsers,  CurrentMessage, userRole);

                // send to all except caller client
                Clients.AllExcept(id).onNewUserConnected(id, userName, userRole, ConnectedUsers);
            }
            else
            {
                var message = String.Format("Логин занят '{0}'", userName);
                Clients.Caller.onError(message);
            }

        }

        public void SendMessageToAll(string userName, string message, string userRole)
        {
            // store last 100 messages in cache
            AddMessageinCache(userName, message, userRole);

            // Broad cast message
            Clients.All.messageReceived(userName, message, userRole);
        }

        public void SendPrivateMessage(string toUserId, string message)
        {

            string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

                // send to caller user
                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
            }

        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
           {
               var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
               if (item != null)
               {
                   ConnectedUsers.Remove(item);

                   var id = Context.ConnectionId;
                   Clients.All.onUserDisconnected(id, item.UserName, ConnectedUsers);

               }

               return base.OnDisconnected(true);
           }
           

        #endregion

        #region private Messages

        private void AddMessageinCache(string userName, string message, string userRole)
        {
            CurrentMessage.Add(new MessageDetail { UserName = userName, Message = message, UserRole = userRole});

            if (CurrentMessage.Count > 100)
                CurrentMessage.RemoveAt(0);
        }

        #endregion
    }

}