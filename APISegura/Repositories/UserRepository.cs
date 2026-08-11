using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Users;
using APISegura.Entities;
using APISegura.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

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
            SELECT Id, Username, Nombre, Email,
                    PasswordHash, PasswordSalt, Iterations, Role,
                    FailedAttempts, LockoutUntil, SecurityStamp
            FROM Users WHERE Username = @Username", conn);

        cmd.Parameters.AddWithValue("@Username", username);

        using var r = await cmd.ExecuteReaderAsync();
        if (!r.Read()) return null;

        return new User
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            Username = r.GetString(r.GetOrdinal("Username")),
            Nombre = r.GetString(r.GetOrdinal("Nombre")),
            Email = r.GetString(r.GetOrdinal("Email")),
            PasswordHash = r.GetString(r.GetOrdinal("PasswordHash")),
            PasswordSalt = r.GetString(r.GetOrdinal("PasswordSalt")),
            Iterations = r.GetInt32(r.GetOrdinal("Iterations")),
            Role = r.GetString(r.GetOrdinal("Role")),
            FailedAttempts = r.GetInt32(r.GetOrdinal("FailedAttempts")),
            LockoutUntil = r.IsDBNull(r.GetOrdinal("LockoutUntil")) ? null : r.GetDateTime(r.GetOrdinal("LockoutUntil")),
            SecurityStamp = r.GetString(r.GetOrdinal("SecurityStamp"))
        };
    }

    public async Task<User?> GetByEmail(string email)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        using var cmd = new SqlCommand(@"
        SELECT TOP 1 *
        FROM Users
        WHERE Email = @Email", conn);

        cmd.Parameters.AddWithValue("@Email", email);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (!reader.Read()) return null;

        return new User
        {
            Id = (int)reader["Id"],
            Username = reader["Username"].ToString(),
            Email = reader["Email"]?.ToString(),
            Nombre = reader["Nombre"].ToString(),
            PasswordHash = reader["PasswordHash"].ToString(),
            PasswordSalt = reader["PasswordSalt"].ToString(),
            Iterations = (int)reader["Iterations"],
            Role = reader["Role"].ToString(),
            FailedAttempts = (int)reader["FailedAttempts"],
            LockoutUntil = reader["LockoutUntil"] as DateTime?,
            SecurityStamp = reader["SecurityStamp"].ToString()
        };
    }

    public async Task<User?> GetById(int id)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand(@"
        SELECT Id, Username, Nombre, Email,
            PasswordHash, PasswordSalt, Iterations, Role,
            FailedAttempts, LockoutUntil, SecurityStamp
        FROM Users WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        using var r = await cmd.ExecuteReaderAsync();
        if (!r.Read()) return null;

        return new User
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            Username = r.GetString(r.GetOrdinal("Username")),
            Nombre = r.GetString(r.GetOrdinal("Nombre")),
            Email = r.GetString(r.GetOrdinal("Email")),
            PasswordHash = r.GetString(r.GetOrdinal("PasswordHash")),
            PasswordSalt = r.GetString(r.GetOrdinal("PasswordSalt")),
            Iterations = r.GetInt32(r.GetOrdinal("Iterations")),
            Role = r.GetString(r.GetOrdinal("Role")),
            FailedAttempts = r.GetInt32(r.GetOrdinal("FailedAttempts")),
            LockoutUntil = r.IsDBNull(r.GetOrdinal("LockoutUntil")) ? null : r.GetDateTime(r.GetOrdinal("LockoutUntil")),
            SecurityStamp = r.GetString(r.GetOrdinal("SecurityStamp"))
        };
    }

    public async Task<int> Create(User user)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand(@"
            INSERT INTO Users (Username, Email, Nombre, PasswordHash, PasswordSalt, Iterations, Role, SecurityStamp)
            VALUES (@Username, @Email, @Nombre, @PasswordHash, @PasswordSalt, @Iterations, @Role, @SecurityStamp);
            SELECT SCOPE_IDENTITY();", conn);

        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@Email", user.Email);
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

    public async Task<PagedResult<UserListDto>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? filtro)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand("FProc_Users_GetPaged", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);
        cmd.Parameters.AddWithValue("@Filtro", (object?)filtro ?? DBNull.Value);

        using var reader = await cmd.ExecuteReaderAsync();

        int totalRegistros = 0;

        if (await reader.ReadAsync())
        {
            totalRegistros = reader.GetInt32(reader.GetOrdinal("TotalRegistros"));
        }

        var usuarios = new List<UserListDto>();

        await reader.NextResultAsync();

        while (await reader.ReadAsync())
        {
            usuarios.Add(new UserListDto
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Email")),
                Role = reader.GetString(reader.GetOrdinal("Role")),
                Active = reader.GetBoolean(reader.GetOrdinal("Active")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            });
        }

        return new PagedResult<UserListDto>
        {
            Items = usuarios,
            TotalRegistros = totalRegistros,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<UserDetailDto?> GetByIdAsync(int id)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand("FProc_Users_GetById", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Id", id);

        using var r = await cmd.ExecuteReaderAsync();

        if (!await r.ReadAsync())
            return null;

        return new UserDetailDto
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            Username = r.GetString(r.GetOrdinal("Username")),
            Nombre = r.GetString(r.GetOrdinal("Nombre")),
            Email = r.IsDBNull(r.GetOrdinal("Email"))
                ? null
                : r.GetString(r.GetOrdinal("Email")),
            Role = r.GetString(r.GetOrdinal("Role")),
            Active = r.GetBoolean(r.GetOrdinal("Active"))
        };
    }

    public async Task<bool> UpdateAsync(
    int id,
    UpdateUserRequest request,
    string updatedBy)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand("FProc_Users_Update", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Nombre", request.Nombre);
        cmd.Parameters.AddWithValue("@Email", (object?)request.Email ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Role", request.Role);
        cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<Result> SetActiveAsync(int id, bool active, string updatedBy)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        var cmd = new SqlCommand("FProc_Users_SetActive", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Active", active);
        cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);

        using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return Result.Failure("No se recibió respuesta del procedimiento.");

        var success = reader.GetInt32(reader.GetOrdinal("Success")) == 1;
        var message = reader.GetString(reader.GetOrdinal("Message"));

        if (success)
        {
            return new Result
            {
                IsSuccess = true,
                Message = message
            };
        }

        return Result.Failure(message);
    }
}