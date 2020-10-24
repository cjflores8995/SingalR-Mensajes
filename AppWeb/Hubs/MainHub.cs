using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AppWeb.Models;
using Business.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace AppWeb.Hubs
{
    public class MainHub : Hub
    {
        private static readonly ConcurrentDictionary<string, UserHub> Users = new ConcurrentDictionary<string, UserHub>(StringComparer.InvariantCultureIgnoreCase);

        public void SendGlobalNotification(string message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
            context.Clients.All.sendGlobalNotification(message);
        }

        public void SendGlobalNotificationExcept(string exceptUserId, string message)
        {
            UserHub receiver;
            if (Users.TryGetValue(exceptUserId, out receiver))
            {
                var cids = receiver.ConnectionIds.ToArray();
                var context = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
                context.Clients.AllExcept(cids).sendGlobalNotificationExcept(message);
            }
        }

        public void DisplayNotification(string toUserId, string texto)
        {
            UserHub receiver;
            if (Users.TryGetValue(toUserId, out receiver))
            {
                var cids = receiver.ConnectionIds.ToList();
                var context = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
                context.Clients.Clients(cids).showNotificaciones(texto);
            }
        }

        public void GetNotification()
        {
            try
            {
                string loggedUserId = Context.User.Identity.GetUserId();

                string totalNotificaciones = LoadNotifData(loggedUserId);

                UserHub receiver;

                if(Users.TryGetValue(loggedUserId, out receiver))
                {
                    var cids = receiver.ConnectionIds.ToList();
                    var context = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

                    context.Clients.Clients(cids).broadcastNotificaciones(totalNotificaciones);
                }
            } 
            catch(Exception ex)
            {
                ex.ToString();
            }
        }

        public void SendNotification(string toUserId)
        {
            try
            {
                string totalNotificaciones = LoadNotifData(toUserId);

                UserHub receiver;
                if (Users.TryGetValue(toUserId, out receiver))
                {
                    var cids = receiver.ConnectionIds.ToList();
                    var context = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
                    context.Clients.Clients(cids).broadcastNotificaciones(totalNotificaciones);
                }
            } 
            catch(Exception ex)
            {
                ex.ToString();
            }
        }

        private string LoadNotifData(string userId)
        {
            int total = 0;
            total = new MensajeRepository().GetCountMessagesFromUserById(userId);

            return total.ToString();
        }

        public override Task OnConnected()
        {
            string userId = Context.User.Identity.GetUserId();
            string connectionId = Context.ConnectionId;

            if (!string.IsNullOrEmpty(userId))
            {
                var user = Users.GetOrAdd(userId, _ => new UserHub
                {
                    UserId = userId,
                    ConnectionIds = new HashSet<string>()
                });

                lock (user.ConnectionIds)
                {
                    user.ConnectionIds.Add(connectionId);
                    if (user.ConnectionIds.Count == 1)
                    {
                        //ojo con este metodo, hay que analizarlo
                        Clients.Others.userConnected(userId);
                    }
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userId = Context.User.Identity.GetUserId();
            string connectionId = Context.ConnectionId;

            if (!string.IsNullOrEmpty(userId))
            {
                UserHub user;
                Users.TryGetValue(userId, out user);

                if (user != null)
                {
                    lock (user.ConnectionIds)
                    {
                        user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));

                        if (!user.ConnectionIds.Any())
                        {
                            UserHub removedUser;
                            Users.TryRemove(userId, out removedUser);
                            Clients.Others.userDisconnected(userId);
                        }
                    }
                }
            }

            
            return base.OnDisconnected(stopCalled);
        }
    }
}