namespace FamHubBack.DTO
{
    public class EventDto
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int UserId { get; set; }
    }
}
