using APISegura.Common;
using APISegura.Dtos.Planta;
using APISegura.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

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
                CodigoOperacional = reader.IsDBNull(2) ? "" : reader.GetString(2)
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
}