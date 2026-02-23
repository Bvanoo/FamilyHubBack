namespace FamHubBack.Data.Entities
{
    public class EventPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string UploaderId { get; set; }
        public int CalendarEventId { get; set; }
    }
}
