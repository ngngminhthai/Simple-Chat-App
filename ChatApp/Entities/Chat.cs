using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; }
        public int SenderId { get; set; }
        public int? GroupChatId { get; set; }
        public int? IndividualChatId { get; set; }

        [ForeignKey("SenderId")]
        public User Sender { get; set; }

        public bool IsDelete { get; set; }

    }
}
