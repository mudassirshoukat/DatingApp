using API.Data;
using API.Extentions;
using API.Helpers.QueryParams;
using API.Interfaces.RepoInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker tracker;
        private readonly IUnitOfWork unitOfWork;

        public PresenceHub(PresenceTracker tracker,IUnitOfWork unitOfWork)
        {
            this.tracker = tracker;
            this.unitOfWork = unitOfWork;
        }


        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var isOnline = await tracker.UserConnected(Context.User.GetUserName(), connectionId);

            if (isOnline)
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserName());

            var Unread = await unitOfWork.MessageRepository
                .GetMessagesForUser(new MessageQueryParams { UserName = Context.User.GetUserName() ,PageSize=50});
            Unread.OrderByDescending(x => x.MessageSent);
            await Clients.Caller.SendAsync("GetUnReadMessages", Unread);

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
