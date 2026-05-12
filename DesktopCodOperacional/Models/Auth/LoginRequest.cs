using System.Text.Json.Serialization;

namespace DesktopCodOperacional.Models.Auth
{
    public class LoginRequest
    {
        [JsonPropertyName("username")]
        public string Usuario { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
