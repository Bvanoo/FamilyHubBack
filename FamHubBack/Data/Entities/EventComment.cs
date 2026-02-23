using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    public class EventComment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PostedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int CalendarEventId { get; set; }
    }
}