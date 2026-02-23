namespace FamHubBack.Data.Entities
{
    public class EventTask
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public int? AssignedUserId { get; set; }
        public int CalendarEventId { get; set; }
        public CalendarEvent CalendarEvent { get; set; }
    }
}