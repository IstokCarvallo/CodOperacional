using System.Security.Cryptography;

namespace APISegura.Services;

public class TokenService
{
    public string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        return Convert.ToBase64String(bytes);
    }
}