using APISegura.Repositories.Interfaces;
using APISegura.Dtos.Common;
using Microsoft.Data.SqlClient;

namespace APISegura.Repositories
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly string _connectionString;

        public AuditoriaRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<AuditoriaDto>> GetAsync(
            DateTime? desde,
            DateTime? hasta,
            string? usuario,
            string? entidad,
            string? accion)
        {
            var result = new List<AuditoriaDto>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"
            SELECT Id, Entidad, Accion, Clave, Campo, ValorAnterior, ValorNuevo, Usuario, Fecha
            FROM dbo.Auditoria
            WHERE (@Desde IS NULL OR Fecha >= @Desde)
              AND (@Hasta IS NULL OR Fecha <= @Hasta)
              AND (@Usuario IS NULL OR Usuario = @Usuario)
              AND (@Entidad IS NULL OR Entidad = @Entidad)
              AND (@Accion IS NULL OR Accion = @Accion)
            ORDER BY Fecha Desc";

            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Desde", (object?)desde ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Hasta", (object?)hasta ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Usuario", (object?)usuario ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Entidad", (object?)entidad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Accion", (object?)accion ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new AuditoriaDto
                {
                    Id = reader.GetInt64(0),
                    Entidad = reader.GetString(1),
                    Accion = reader.GetString(2),
                    Clave = reader.GetString(3),
                    Campo = reader.GetString(4),
                    ValorAnterior = reader.IsDBNull(5) ? null : reader.GetString(5),
                    ValorNuevo = reader.IsDBNull(6) ? null : reader.GetString(6),
                    Usuario = reader.GetString(7),
                    Fecha = reader.GetDateTime(8)
                });
            }

            return result;
        }

        public async Task<AuditoriaDto?> GetByIdAsync(long id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"SELECT Id, Entidad, Accion, Clave, Campo, ValorAnterior, ValorNuevo, Usuario, Fecha
                      FROM dbo.Auditoria WHERE Id = @Id";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new AuditoriaDto
                {
                    Id = reader.GetInt64(0),
                    Entidad = reader.GetString(1),
                    Accion = reader.GetString(2),
                    Clave = reader.GetString(3),
                    Campo = reader.GetString(4),
                    ValorAnterior = reader.IsDBNull(5) ? null : reader.GetString(5),
                    ValorNuevo = reader.IsDBNull(6) ? null : reader.GetString(6),
                    Usuario = reader.GetString(7),
                    Fecha = reader.GetDateTime(8)
                };
            }

            return null;
        }
    }
}
