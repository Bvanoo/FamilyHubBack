using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    [Table("GroupMembers")]
    public class GroupMember
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Role { get; set; } = null!;
        public DateTime JoinedAt { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; } = null!;
    }
}
