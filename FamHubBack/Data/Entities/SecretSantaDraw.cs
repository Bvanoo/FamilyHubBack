using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    public class SecretSantaDraw
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; }

        public int GiverId { get; set; }
        [ForeignKey("GiverId")]
        public User Giver { get; set; }

        public int ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public User Receiver { get; set; }

        public DateTime DrawDate { get; set; } = DateTime.UtcNow;
    }
}