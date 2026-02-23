namespace FamHubBack.Data.Entities
{
    public class EventPoll
    {
        public int Id { get; set; }
        public string Question { get; set; }

        public int CalendarEventId { get; set; }

        public ICollection<PollOption> Options { get; set; }
    }
}