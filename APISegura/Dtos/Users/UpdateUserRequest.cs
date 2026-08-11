namespace APISegura.Dtos.Users
{
    public sealed class UpdateUserRequest
    {
        public string Nombre { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string Role { get; set; } = string.Empty;
    }
}