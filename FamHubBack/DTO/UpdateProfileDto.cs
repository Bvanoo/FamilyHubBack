namespace FamHubBack.DTO
{
    public class UpdateProfileDto
    {
        public string Name { get; set; } = null!;
        public IFormFile? File { get; set; }
    }
}
