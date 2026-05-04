namespace APISegura.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime Created { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? ReplacedByToken { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public string? Device { get; set; }

    public DateTime? LastUsedAt { get; set; }

    public string? RevokedReason { get; set; }
}