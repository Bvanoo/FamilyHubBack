using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    public class EventParticipant
    {
        public int Id { get; set; }
        public int CalendarEventId { get; set; }
        public CalendarEvent CalendarEvent { get; set; }
        public int UserId { get; set; } 
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string Status { get; set; } = "Pending";
    }
}