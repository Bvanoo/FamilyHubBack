namespace FamHubBack.Data.Entities
{
    public class EventTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public int EventId { get; set; }
        public CalendarEvent Event { get; set; }
        public ICollection<User> AssignedUsers { get; set; } = new List<User>();
    }
}