using Microsoft.Data.SqlClient;
using APISegura.Entities;

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
            SELECT Id, Username, PasswordHash, PasswordSalt, Iterations, Role
            FROM Users WHERE Username = @Username", conn);

        cmd.Parameters.AddWithValue("@Username", username);

        using var r = await cmd.ExecuteReaderAsync();
        if (!r.Read()) return null;

        return new User
        {
            Id = r.GetInt32(0),
            Username = r.GetString(1),
            PasswordHash = r.GetString(2),
            PasswordSalt = r.GetString(3),
            Iterations = r.GetInt32(4),
            Role = r.GetString(5)
        };
    }

    public async Task<User?> GetById(int id)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand(@"
        SELECT Id, Username, PasswordHash, PasswordSalt, Iterations, Role
        FROM Users WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        using var r = await cmd.ExecuteReaderAsync();
        if (!r.Read()) return null;

        return new User
        {
            Id = r.GetInt32(0),
            Username = r.GetString(1),
            PasswordHash = r.GetString(2),
            PasswordSalt = r.GetString(3),
            Iterations = r.GetInt32(4),
            Role = r.GetString(5)
        };
    }

    public async Task<int> Create(User user)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand(@"
            INSERT INTO Users (Username, PasswordHash, PasswordSalt, Iterations, Role)
            VALUES (@Username, @PasswordHash, @PasswordSalt, @Iterations, @Role);
            SELECT SCOPE_IDENTITY();", conn);

        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        cmd.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
        cmd.Parameters.AddWithValue("@Iterations", user.Iterations);
        cmd.Parameters.AddWithValue("@Role", user.Role);

        var id = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(id);
    }
}