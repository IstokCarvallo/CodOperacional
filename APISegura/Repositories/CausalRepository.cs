using APISegura.Common;
using APISegura.Dtos.Causales;
using APISegura.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APISegura.Repositories
{
    public class CausalRepository : ICausalRepository
    {
        private readonly IConfiguration _config;

        public CausalRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Result<List<CausalDto>>> GetByEspecieAsync(int espeCodigo, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var command = new CommandDefinition(
                "dbo.SP_Causales_GetByEspecie",
                new
                {
                    EspeCodigo = espeCodigo
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            var result = await connection.QueryAsync<CausalDto>(
                command);

            return Result<List<CausalDto>>.Ok(
                result.AsList());
        }

        public async Task<Result<int>> CreateAsync(CreateCausalRequest request, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var command = new CommandDefinition(
                "dbo.SP_Causales_Create",
                new
                {
                    request.EspeCodigo,
                    request.Codigo,
                    request.Descripcion,
                    request.Tipo
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            var causalId = await connection.QuerySingleAsync<int>(
                command);

            return Result<int>.Ok(causalId);
        }

        public async Task<Result> SetActiveAsync(int causalId, bool activo, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var command = new CommandDefinition(
                "dbo.SP_Causales_SetActive",
                new
                {
                    CausalId = causalId,
                    Activo = activo
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            await connection.ExecuteAsync(command);

            return Result.Success();
        }
    }
}
