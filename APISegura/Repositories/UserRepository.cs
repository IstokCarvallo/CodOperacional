using Microsoft.Data.SqlClient;
using APISegura.Entities;
using APISegura.Repositories.Interfaces;

namespace APISegura.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IConfiguration _config;

    public UserRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<User?> GetByUsername(string username)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand(@"
            SELECT Id, Username, Nombre,
                    PasswordHash, PasswordSalt, Iterations, Role,
                    FailedAttempts, LockoutUntil, SecurityStamp
            FROM Users WHERE Username = @Username", conn);

        cmd.Parameters.AddWithValue("@Username", username);

        using var r = await cmd.ExecuteReaderAsync();
        if (!r.Read()) return null;

        return new User
        {
            Id = r.GetInt32(0),
            Username = r.GetString(1),
            Nombre = r.GetString(2),
            PasswordHash = r.GetString(3),
            PasswordSalt = r.GetString(4),
            Iterations = r.GetInt32(5),
            Role = r.GetString(6),
            FailedAttempts = r.GetInt32(7),
            LockoutUntil = r.IsDBNull(8) ? null : r.GetDateTime(8),
            SecurityStamp = r.GetString(9)
        };
    }

    public async Task<User?> GetById(int id)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand(@"
        SELECT Id, Username, Nombre,
            PasswordHash, PasswordSalt, Iterations, Role,
            FailedAttempts, LockoutUntil, SecurityStamp
        FROM Users WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        using var r = await cmd.ExecuteReaderAsync();
        if (!r.Read()) return null;

        return new User
        {
            Id = r.GetInt32(0),
            Username = r.GetString(1),
            Nombre = r.GetString(2),
            PasswordHash = r.GetString(3),
            PasswordSalt = r.GetString(4),
            Iterations = r.GetInt32(5),
            Role = r.GetString(6),
            FailedAttempts = r.GetInt32(7),
            LockoutUntil = r.IsDBNull(8) ? null : r.GetDateTime(8),
            SecurityStamp = r.GetString(9)
        };
    }

    public async Task<int> Create(User user)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand(@"
            INSERT INTO Users (Username, Nombre, PasswordHash, PasswordSalt, Iterations, Role, SecurityStamp)
            VALUES (@Username, @Nombre, @PasswordHash, @PasswordSalt, @Iterations, @Role, @SecurityStamp);
            SELECT SCOPE_IDENTITY();", conn);

        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@Nombre", user.Nombre);
        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        cmd.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
        cmd.Parameters.AddWithValue("@Iterations", user.Iterations);
        cmd.Parameters.AddWithValue("@Role", user.Role);
        cmd.Parameters.AddWithValue("@SecurityStamp", user.SecurityStamp);

        var id = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(id);
    }

    public async Task Update(User user)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var query = @"
            UPDATE Users
            SET 
                FailedAttempts = @FailedAttempts,
                LockoutUntil = @LockoutUntil
            WHERE Id = @Id
        ";

        using var cmd = new SqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@FailedAttempts", user.FailedAttempts);
        cmd.Parameters.AddWithValue("@LockoutUntil", (object?)user.LockoutUntil ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Id", user.Id);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdatePassword(User user)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var query = @"
        UPDATE Users
        SET 
            PasswordHash = @PasswordHash,
            PasswordSalt = @PasswordSalt,
            Iterations = @Iterations
        WHERE Id = @Id";

        using var cmd = new SqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        cmd.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
        cmd.Parameters.AddWithValue("@Iterations", user.Iterations);
        cmd.Parameters.AddWithValue("@Id", user.Id);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateSecurityStamp(User user)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var query = @"
        UPDATE Users
        SET SecurityStamp = @SecurityStamp
        WHERE Id = @Id";

        using var cmd = new SqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@SecurityStamp", user.SecurityStamp);
        cmd.Parameters.AddWithValue("@Id", user.Id);

        await cmd.ExecuteNonQueryAsync();
    }
}