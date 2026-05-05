namespace APISegura.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Nombre { get; set; } 
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public int Iterations { get; set; }
    public string Role { get; set; }
    public int FailedAttempts { get; set; }
    public DateTime? LockoutUntil { get; set; }
    public string SecurityStamp { get; set; }
}