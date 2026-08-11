namespace FrontCodOperacional.Models.Users
{
    public sealed class UserDetailDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string Role { get; set; } = string.Empty;

        public bool Active { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}