using API.DTO.MessageDtos;
using API.Entities;
using API.Extentions;
using API.Interfaces.RepoInterfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi;

namespace API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHubContext<PresenceHub> presenceHub;

        public MessageHub(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHubContext<PresenceHub> presenceHub)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.presenceHub = presenceHub;
        }

        public async override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"];
            var groupName = GetGroupName(Context.User.GetUserName(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);


            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

          

            var messages = await unitOfWork.MessageRepository.GetMessageThread(Context.User.GetUserName(), otherUser);
            if (unitOfWork.HasChanges()) await unitOfWork.Complete();
            await Clients.Caller.SendAsync("GetMessagesThread", messages);

        }


        public async Task NewMessage(CreateMessageDto messageDto)
        {
            var CurrentUserName = Context.User.GetUserName();
            if (CurrentUserName.ToLower() == messageDto.RecipientUserName.ToLower()) throw new HubException("You Connot Send message To Youself");

            var Sender = await unitOfWork.UserRepository.GetUserByUserNameAsync(CurrentUserName);
            var Recipient = await unitOfWork.UserRepository.GetUserByUserNameAsync(messageDto.RecipientUserName);
            if (Recipient == null) throw new HubException("Not Found");
            var message = new Message
            {
                Sender = Sender,
                SenderUserName = CurrentUserName,
                Recipient = Recipient,
                RecipientUserName = Recipient.UserName,
                Content = messageDto.Content,
            };

            var groupName = GetGroupName(CurrentUserName, Recipient.UserName);
            var group = await unitOfWork.MessageRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.UserName == Recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await PresenceTracker.GetConnectionsForUser(Recipient.UserName);

                if (connections != null)
                {
                    await presenceHub.Clients.Clients(connections).SendAsync("NewMessageNotification", new { UserName = Sender.UserName, KnownAs = Sender.KnownAs });
                }



            }




            unitOfWork.MessageRepository.AddMessage(message);
            if (await unitOfWork.Complete())
            {

                await Clients.Group(groupName).SendAsync("NewMessage", mapper.Map<MessageDto>(message));

            }

        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup");
            await base.OnDisconnectedAsync(exception);
        }





        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }


        private async Task<Group> AddToGroup(string GroupName)
        {
            var group = await unitOfWork.MessageRepository.GetMessageGroup(GroupName);
            if (group == null)
            {
                group = new Group(GroupName);
                unitOfWork.MessageRepository.AddGroup(group);
            }
            group.Connections.Add(new Connection(Context.ConnectionId, Context.User.GetUserName()));
            if (await unitOfWork.Complete()) return group;
            throw new HubException("Failed To Add To Group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await unitOfWork.MessageRepository.GetGroupFromConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            unitOfWork.MessageRepository.RemoveConnection(connection);

            if (await unitOfWork.Complete()) return group;
            throw new HubException("Failed To Remove From Group");

        }
    }
}
