using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Entities
{
    public class GroupChat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Chat> Chats { get; set; }

        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
        public int CreatorId { get; set; }
    }
}
