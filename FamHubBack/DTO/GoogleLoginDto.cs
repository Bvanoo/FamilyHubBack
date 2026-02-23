using System.Text.Json.Serialization;

namespace FamHubBack.DTO
{
    public class GoogleLoginDto
    {
        [JsonPropertyName("token")]
        public string IdToken { get; set; } = null!;
    }
}
