using API.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker tracker;

        public PresenceHub(PresenceTracker tracker)
        {
            this.tracker = tracker;
        }


        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var isOnline = await tracker.UserConnected(Context.User.GetUserName(), connectionId);

            if (isOnline)
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserName());


            var connectedUsers = tracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", connectedUsers);

        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {

            var isOffline = await tracker.UserDisconnected(Context.User.GetUserName(), Context.ConnectionId);
            if (isOffline)
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUserName());

          

            await base.OnDisconnectedAsync(ex);
        }
    }
}
