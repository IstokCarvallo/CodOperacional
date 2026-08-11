using APISegura.Common;
using APISegura.Dtos.EtiquetasTAG;
using APISegura.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APISegura.Repositories;

public class EtiquetaTAGRepository : IEtiquetaTAGRepository
{
    private readonly IConfiguration _config;

    public EtiquetaTAGRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<Result<List<EtiquetaTAGDto>>> SearchAsync(
        string? filtro,
        CancellationToken cancellationToken)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        var command = new CommandDefinition("dbo.SP_EtiquetasTAG_Search",
            new
            {
                Filtro = filtro
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var result = await connection.QueryAsync<EtiquetaTAGDto>(command);

        return Result<List<EtiquetaTAGDto>>.Ok(
            result.AsList());
    }

    public async Task<Result<int>> CreateAsync(
        CreateEtiquetaTAGRequest request,
        CancellationToken cancellationToken)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        var command = new CommandDefinition("dbo.SP_EtiquetasTAG_Create",
            new
            {
                request.Nombre,
                request.Descripcion
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var etiquetaId = await connection.QuerySingleAsync<int>(command);

        return Result<int>.Ok(etiquetaId);
    }
}