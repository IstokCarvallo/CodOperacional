using Microsoft.Data.SqlClient;
using APISegura.Entities;

namespace APISegura.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IConfiguration _config;

    public RefreshTokenRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task Save(RefreshToken token)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand(@"
            INSERT INTO RefreshTokens (UserId, Token, Expiration)
            VALUES (@UserId, @Token, @Expiration)", conn);

        cmd.Parameters.AddWithValue("@UserId", token.UserId);
        cmd.Parameters.AddWithValue("@Token", token.Token);
        cmd.Parameters.AddWithValue("@Expiration", token.Expiration);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<RefreshToken?> Get(string token)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand(@"
            SELECT Id, UserId, Token, Expiration, IsRevoked
            FROM RefreshTokens
            WHERE Token = @Token", conn);

        cmd.Parameters.AddWithValue("@Token", token);

        using var r = await cmd.ExecuteReaderAsync();

        if (!r.Read()) return null;

        return new RefreshToken
        {
            Id = r.GetInt32(0),
            UserId = r.GetInt32(1),
            Token = r.GetString(2),
            Expiration = r.GetDateTime(3),
            IsRevoked = r.GetBoolean(4)
        };
    }

    public async Task Revoke(string token)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand(@"
            UPDATE RefreshTokens
            SET IsRevoked = 1
            WHERE Token = @Token", conn);

        cmd.Parameters.AddWithValue("@Token", token);

        await cmd.ExecuteNonQueryAsync();
    }
}