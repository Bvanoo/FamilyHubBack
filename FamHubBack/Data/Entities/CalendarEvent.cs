using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    [Table("CalendarEvents")]
    public class CalendarEvent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public string? Type { get; set; }
        public string? Color { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }
        public bool IsPrivateEvent { get; set; }
        public bool MaskDetails { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group? Group { get; set; }

        public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();
        public ICollection<EventTask> Tasks { get; set; } = new List<EventTask>();
        public ICollection<EventPoll> Polls { get; set; } = new List<EventPoll>();
        public ICollection<EventComment> Comments { get; set; } = new List<EventComment>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}