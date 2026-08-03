using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Cuarteles;
using APISegura.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APISegura.Repositories
{
    public class CuartelRepository : ICuartelRepository
    {
        private readonly IConfiguration _config;

        public CuartelRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<List<CatalogoDto>> GetProductoresAsync(string? filtro)
        {
            var list = new List<CatalogoDto>();

            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using var cmd = new SqlCommand("dbo.FProc_Productor_Search", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Filtro", (object?)filtro ?? DBNull.Value);

            using var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync())
            {
                list.Add(new CatalogoDto
                {
                    Codigo = r.GetInt32(0),
                    Nombre = r.IsDBNull(1) ? "" : r.GetString(1)
                });
            }

            return list;
        }

        public async Task<List<CatalogoDto>> GetPrediosAsync(int productor, string? filtro)
        {
            var list = new List<CatalogoDto>();

            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using var cmd = new SqlCommand("dbo.FProc_Predio_Search", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Productor", productor);
            cmd.Parameters.AddWithValue("@Filtro", (object?)filtro ?? DBNull.Value);

            using var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync())
            {
                list.Add(new CatalogoDto
                {
                    Codigo = r.GetInt32(0),
                    Nombre = r.IsDBNull(1) ? "" : r.GetString(1)
                });
            }

            return list;
        }

        public async Task<List<CuartelDto>>SearchAsync(int productor, int predio, string? filtro)
        {
            var result = new List<CuartelDto>();

            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using var cmd = new SqlCommand("dbo.FProc_Cuartel_Search", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Productor", productor);
            cmd.Parameters.AddWithValue("@Predio", predio);
            cmd.Parameters.AddWithValue("@Filtro", (object?)filtro ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new CuartelDto
                {
                    Productor = reader.GetInt32(0),
                    Predio = reader.GetInt32(1),
                    CodigoCuartel = reader.GetInt32(2),
                    Nombre = reader.GetString(3),
                    CodigoOperacional = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Especie = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Variedad = reader.IsDBNull(6) ? "" : reader.GetString(6),
                    FechaUltimaActualizacion = reader.IsDBNull(reader.GetOrdinal("FechaUltimaActualizacion"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("FechaUltimaActualizacion"))
                });
            }

            return result;
        }

        public async Task<Result> UpdateCodigoOperacionalAsync(
            int productor,
            int predio,
            int codigoCuartel,
            string nuevoCodigo,
            string usuario)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using var cmd = new SqlCommand("dbo.FProc_Cuartel_UpdateCodigoOperacional", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Productor", productor);
            cmd.Parameters.AddWithValue("@Predio", predio);
            cmd.Parameters.AddWithValue("@CodigoCuartel", codigoCuartel);
            cmd.Parameters.AddWithValue("@NuevoCodigoOperacional", nuevoCodigo);
            cmd.Parameters.AddWithValue("@Usuario", usuario);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return reader.GetInt32(0) == 1
                    ? Result.Success()
                    : Result.Failure(reader.GetString(1));
            }

            return Result.Failure("Respuesta inválida");
        }
    }
}
