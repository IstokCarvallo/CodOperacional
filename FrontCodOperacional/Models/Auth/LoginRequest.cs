using System.Text.Json.Serialization;

namespace FrontCodOperacional.Models.Auth
{
    public class LoginRequest
    {
        [JsonPropertyName("username")]
        public string Usuario { get; set; } = "";
        [JsonPropertyName("password")]
        public string Password { get; set; } = "";
    }
}
