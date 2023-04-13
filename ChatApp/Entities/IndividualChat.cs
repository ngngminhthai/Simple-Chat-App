using System.ComponentModel.DataAnnotations.Schema;


namespace ChatApp.Entities
{
    public class IndividualChat
    {
        public int Id { get; set; }

        [NotMapped]
        public string DisplayName { get; set; }

        [ForeignKey("UserOneId")]
        public User UserOne { get; set; }
        public int UserOneId { get; set; }

        [ForeignKey("UserTwoId")]
        public User UserTwo { get; set; }
        public int UserTwoId { get; set; }

        public List<Chat> Chats { get; set; }
    }
}
