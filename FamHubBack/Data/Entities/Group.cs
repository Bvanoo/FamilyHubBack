using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    [Table("Groups")]
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public User Owner { get; set; } = null!;

        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    }
}
