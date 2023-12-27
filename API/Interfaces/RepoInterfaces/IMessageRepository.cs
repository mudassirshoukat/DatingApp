using API.DTO.MessageDtos;
using API.Entities;
using API.Helpers;
using API.Helpers.QueryParams;

namespace API.Interfaces.RepoInterfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int Id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageQueryParams prms);
        Task<IEnumerable<MessageDto>> GetMessageThread(string CurrentUserName,string RecipientUserName);
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetMessageGroup(string GroupName);
        Task<Group> GetGroupFromConnection(string ConnectionId);
        Task<bool> SaveAllAsync();

    }
}
