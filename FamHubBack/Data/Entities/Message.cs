using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    [Table("Send")]
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int SenderId { get; set; }
        [ForeignKey("SenderId")]
        public User Sender { get; set; } = null!;

        public int ConversationId { get; set; }
        [ForeignKey("ConversationId")]
        public Conversation Conversation { get; set; } = null!;
    }
}
