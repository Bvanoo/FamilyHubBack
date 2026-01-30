using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    [Table("conversations")]
    public class Conversation
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Type { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int? GroupId { get; set; }

        public ICollection<ConversationMember> Members { get; set; } = new List<ConversationMember>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
