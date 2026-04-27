using APISegura.Common;
using APISegura.Dtos.Planta;
using APISegura.Entities;
using APISegura.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Numerics;
using System.Reflection.PortableExecutable;

namespace APISegura.Repositories;

public class PlantaRepository : IPlantaRepository
{
    private readonly IConfiguration _config;

    public PlantaRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<List<PlantaDto>> SearchAsync(string? filtro)
    {
        var result = new List<PlantaDto>();

        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        using var cmd = new SqlCommand("dbo.Fproc_Planta_Search", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Filtro", (object?)filtro ?? DBNull.Value);

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new PlantaDto
            {
                Codigo = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                CodigoOperacional = reader.IsDBNull(2) ? "" : reader.GetString(2),
                FechaUltimaActualizacion = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3)
            });
        }

        return result;
    }

    public async Task<Result> UpdateCodigoOperacionalAsync(int codigo, string nuevoCodigo, string usuario)
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await conn.OpenAsync();

        using var cmd = new SqlCommand("dbo.FPRoc_Planta_UpdateCodigoOperacional", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Codigo", codigo);
        cmd.Parameters.AddWithValue("@NuevoCodigoOperacional", nuevoCodigo);
        cmd.Parameters.AddWithValue("@Usuario", usuario);

        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var success = reader.GetInt32(0) == 1;
            var message = reader.GetString(1);

            return success
                ? Result.Success()
                : Result.Failure(message);
        }

        return Result.Failure("Respuesta inválida del servidor");
    }

    public async Task<(IEnumerable<Planta>, int)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var plantas = new List<Planta>();
        int total = 0;

        using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        using (var command = new SqlCommand("dbo.FProc_Plantas_GetPaged", connection))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            await connection.OpenAsync();

            using (var reader = await command.ExecuteReaderAsync())
            {
                // Total
                if (await reader.ReadAsync())
                    total = reader[0] != DBNull.Value ? Convert.ToInt32(reader[0]) : 0;

                // Resultset datos
                await reader.NextResultAsync();

                while (await reader.ReadAsync())
                {
                    plantas.Add(new Planta
                    {
                        Codigo = reader["plde_codigo"] != DBNull.Value ? Convert.ToInt32(reader["plde_codigo"]) : 0,
                        Nombre = reader["plde_nombre"] != DBNull.Value ? reader["plde_nombre"].ToString() : string.Empty,
                        FechaUltimaActualizacion = reader.IsDBNull(reader.GetOrdinal("FechaUltimaActualizacion"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("FechaUltimaActualizacion"))
                    });
                    
                }
            }
        }
        return (plantas, total);
    }
}