using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Group
    {
        public Group(string name)
        {
            Name = name;
        }

        [Key]
        public string Name { get; set; }

        //nivagation properties

        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    }
}
