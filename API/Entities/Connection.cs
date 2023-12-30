
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Connection
    {
        public Connection()
        {

        }

        public Connection(string connectionId, string username)
        {
            ConnectionId = connectionId;
            UserName = username;
        }
        [Key]
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
    }
}
