namespace FrontCodOperacional.Models.User
{
    public sealed class CurrentUserInfo
    {
        public int UserId { get; init; }

        public string Username { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;

        public string Role { get; init; } = string.Empty;

        public string Initials { get; init; } = string.Empty;
    }
}
