using System.Security.Cryptography;

namespace APISegura.Services;

public class PasswordService
{
    private const int SaltSize = 16;      // 128 bits
    private const int KeySize = 32;       // 256 bits
    private const int DefaultIterations = 10000;

    public (string hash, string salt, int iterations) HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[SaltSize];
        rng.GetBytes(saltBytes);

        var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            DefaultIterations,
            HashAlgorithmName.SHA256,
            KeySize);

        return (
            Convert.ToBase64String(hashBytes),
            Convert.ToBase64String(saltBytes),
            DefaultIterations
        );
    }

    public bool Verify(string password, string storedHash, string storedSalt, int iterations)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        var hashBytes = Convert.FromBase64String(storedHash);

        var computed = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            iterations,
            HashAlgorithmName.SHA256,
            hashBytes.Length);

        return CryptographicOperations.FixedTimeEquals(hashBytes, computed);
    }
}