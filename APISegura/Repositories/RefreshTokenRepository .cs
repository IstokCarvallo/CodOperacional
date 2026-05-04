using Microsoft.Data.SqlClient;
using APISegura.Entities;
using APISegura.Repositories.Interfaces;

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
            INSERT INTO RefreshTokens (UserId, Token, Expiration, IsRevoked,
                    IpAddress, UserAgent, Device, LastUsedAt, RevokedReason)
            VALUES (@UserId, @Token, @Expiration, 0,
                    @IpAddress, @UserAgent, @Device, @LastUsedAt, @RevokedReason)", conn);

        cmd.Parameters.AddWithValue("@UserId", token.UserId);
        cmd.Parameters.AddWithValue("@Token", token.Token);
        cmd.Parameters.AddWithValue("@Expiration", token.Expiration);
        cmd.Parameters.AddWithValue("@IpAddress", (object?)token.IpAddress ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@UserAgent", (object?)token.UserAgent ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Device", (object?)token.Device ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@LastUsedAt", (object?)token.LastUsedAt ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@RevokedReason", (object?)token.RevokedReason ?? DBNull.Value);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<RefreshToken?> Get(string token)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand(@"
            SELECT Id, UserId, Token, Expiration, IsRevoked, Created,
               RevokedAt, ReplacedByToken,
               IpAddress, UserAgent, Device, LastUsedAt, RevokedReason
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
            IsRevoked = r.GetBoolean(4),
            Created = r.GetDateTime(5),

            RevokedAt = r.IsDBNull(6) ? null : r.GetDateTime(6),
            ReplacedByToken = r.IsDBNull(7) ? null : r.GetString(7),

            IpAddress = r.IsDBNull(8) ? null : r.GetString(8),
            UserAgent = r.IsDBNull(9) ? null : r.GetString(9),
            Device = r.IsDBNull(10) ? null : r.GetString(10),
            LastUsedAt = r.IsDBNull(11) ? null : r.GetDateTime(11),
            RevokedReason = r.IsDBNull(12) ? null : r.GetString(12)
        };
    }

    public async Task Update(RefreshToken token)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var query = @"
            UPDATE RefreshTokens
            SET IsRevoked = @IsRevoked,
                RevokedAt = @RevokedAt,
                ReplacedByToken = @ReplacedByToken,
                LastUsedAt = @LastUsedAt,
                RevokedReason = @RevokedReason
            WHERE Token = @Token";

        using var cmd = new SqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@IsRevoked", token.IsRevoked);
        cmd.Parameters.AddWithValue("@RevokedAt", (object?)token.RevokedAt ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ReplacedByToken", (object?)token.ReplacedByToken ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@LastUsedAt", (object?)token.LastUsedAt ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@RevokedReason", (object?)token.RevokedReason ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Token", token.Token);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<RefreshToken>> GetActiveByUser(int userId)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var query = @"
        SELECT *
        FROM RefreshTokens
        WHERE UserId = @UserId
          AND IsRevoked = 0
          AND Expiration > GETUTCDATE()";

        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@UserId", userId);

        var list = new List<RefreshToken>();

        using var r = await cmd.ExecuteReaderAsync();

        while (r.Read())
        {
            list.Add(new RefreshToken
            {
                Id = r.GetInt32(r.GetOrdinal("Id")),
                UserId = r.GetInt32(r.GetOrdinal("UserId")),
                Token = r.GetString(r.GetOrdinal("Token")),
                Created = r.GetDateTime(r.GetOrdinal("Created")),
                LastUsedAt = r.IsDBNull(r.GetOrdinal("LastUsedAt")) ? null : r.GetDateTime(r.GetOrdinal("LastUsedAt")),
                IpAddress = r.IsDBNull(r.GetOrdinal("IpAddress")) ? null : r.GetString(r.GetOrdinal("IpAddress")),
                Device = r.IsDBNull(r.GetOrdinal("Device")) ? null : r.GetString(r.GetOrdinal("Device"))
            });
        }

        return list;
    }

    public async Task RevokeAllByUser(int userId)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var query = @"
        UPDATE RefreshTokens
        SET IsRevoked = 1,
            RevokedAt = GETUTCDATE(),
            RevokedReason = 'LogoutAll'
        WHERE UserId = @UserId
          AND IsRevoked = 0";

        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@UserId", userId);

        await cmd.ExecuteNonQueryAsync();
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

    public async Task RevokeAllExcept(int userId, string currentToken)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var query = @"
        UPDATE RefreshTokens
        SET IsRevoked = 1,
            RevokedAt = GETUTCDATE(),
            RevokedReason = 'LogoutOthers'
        WHERE UserId = @UserId
          AND Token <> @Token
          AND IsRevoked = 0";

        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@Token", currentToken);

        await cmd.ExecuteNonQueryAsync();
    }    
}