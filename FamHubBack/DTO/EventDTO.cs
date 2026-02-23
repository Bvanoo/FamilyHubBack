namespace FamHubBack.DTO
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int UserId { get; set; }
        public bool IsPrivate { get; set; }
        public bool MaskDetails { get; set; }
        public int? GroupId { get; set; }
    }
}