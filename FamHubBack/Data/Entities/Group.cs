using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public bool IsPublic { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string InviteCode { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
        public ICollection<CalendarEvent> Events { get; set; } = new List<CalendarEvent>();
        public ICollection<Message> Messages { get; set; }
    }
}