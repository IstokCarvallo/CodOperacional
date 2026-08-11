namespace APISegura.Dtos.Users    
{
    public sealed class UserListDto
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