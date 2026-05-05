using APISegura.Entities;
using APISegura.Repositories.Interfaces;
using APISegura.Services;
using Microsoft.Data.SqlClient;

namespace APISegura.Repositories
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly string _connectionString;
        private readonly TokenService _tokenService;

        public PasswordResetRepository(IConfiguration config, 
                                    TokenService tokenService)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _tokenService = tokenService;
        }

        public async Task Create(PasswordResetToken token)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
            INSERT INTO PasswordResetTokens (UserId, TokenHash, Expiration)
            VALUES (@UserId, @TokenHash, @Expiration)", conn);

            cmd.Parameters.AddWithValue("@UserId", token.UserId);
            cmd.Parameters.AddWithValue("@TokenHash", token.TokenHash);
            cmd.Parameters.AddWithValue("@Expiration", token.Expiration);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<PasswordResetToken?> GetByToken(string token)
        {
            var hash = _tokenService.HashToken(token);
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
            SELECT TOP 1 *
            FROM PasswordResetTokens
            WHERE TokenHash = @TokenHash", conn);

            cmd.Parameters.AddWithValue("@TokenHash", hash);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!reader.Read()) return null;

            return new PasswordResetToken
            {
                Id = (int)reader["Id"],
                UserId = (int)reader["UserId"],
                TokenHash = reader["TokenHash"].ToString(),
                Expiration = (DateTime)reader["Expiration"],
                IsUsed = (bool)reader["IsUsed"],
                CreatedAt = (DateTime)reader["CreatedAt"]
            };
        }

        public async Task InvalidateAllByUser(int userId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
            UPDATE PasswordResetTokens
            SET IsUsed = 1
            WHERE UserId = @UserId AND IsUsed = 0", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Update(PasswordResetToken token)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
            UPDATE PasswordResetTokens
            SET IsUsed = @IsUsed
            WHERE Id = @Id", conn);

            cmd.Parameters.AddWithValue("@Id", token.Id);
            cmd.Parameters.AddWithValue("@IsUsed", token.IsUsed);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
