namespace FamHubBack.DTO
{
    public class EventTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public List<string> AssignedUserNames { get; set; } = new List<string>();
    }
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public List<string> AssignedUserIds { get; set; } = new List<string>();
    }
}
