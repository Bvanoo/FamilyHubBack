namespace FamHubBack.Data.Entities
{
    public class Tricount
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int? GroupId { get; set; }

        public int CreatorId { get; set; }

        public User Creator { get; set; } = null!;
    }
}
