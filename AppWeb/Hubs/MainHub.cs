using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AppWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace AppWeb.Hubs
{
    public class MainHub : Hub
    {
        private static readonly ConcurrentDictionary<string, UserHub> Users = new ConcurrentDictionary<string, UserHub>(StringComparer.InvariantCultureIgnoreCase);

        public override Task OnConnected()
        {
            string userId = Context.User.Identity.GetUserId();
            string connectionId = Context.ConnectionId;

            var user = Users.GetOrAdd(userId, _ => new UserHub
            {
                UserId = userId,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(connectionId);
                if(user.ConnectionIds.Count == 1)
                {
                    //ojo con este metodo, hay que analizarlo
                    Clients.Others.userConnected(userId);
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userId = Context.User.Identity.GetUserId();
            string connectionId = Context.ConnectionId;

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

            return base.OnDisconnected(stopCalled);
        }
    }
}