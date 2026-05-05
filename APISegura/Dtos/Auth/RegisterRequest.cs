namespace APISegura.Dtos.Auth;

public class RegisterRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Nombre { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "User";
}