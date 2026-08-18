using APISegura.Common;
using APISegura.Dtos.Causales;
using APISegura.Dtos.Common;
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

        public async Task<Result<List<CatalogoDto>>> GetEspeciesAsync(
                                string? filtro,
                                CancellationToken ct)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var command = new CommandDefinition("dbo.SP_Especies_Search",
                new
                {
                    Filtro = filtro
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct);

            var result = await connection.QueryAsync<CatalogoDto>(command);

            return Result<List<CatalogoDto>>.Ok(
                result.AsList());
        }

        public async Task<(IEnumerable<CausalDto> Causales, int TotalRegistros)> GetByEspecieAsync(
            int Codigo,
            int PageNumber,
            int PageSize,
            string? Filtro,
            CancellationToken ct)
        {
            using var connection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var command = new CommandDefinition("dbo.SP_Causales_GetByEspecie",
                new
                {
                    Codigo = Codigo,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    Filtro = Filtro
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct);

            using var multi = await connection.QueryMultipleAsync(command);

            
            var totalRegistros = await multi.ReadSingleAsync<int>();

            var causales = (await multi.ReadAsync<CausalDto>()).AsList();

            return (causales, totalRegistros);
        }

        public async Task<Result<int>> CreateAsync(CreateCausalRequest request, string usuario, CancellationToken ct)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var command = new CommandDefinition(
                "dbo.SP_Causales_Create",
                new
                {
                    request.Codigo,
                    request.Nombre,
                    usuario
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct);

            var result = await connection.QuerySingleAsync<CreateCausalResult>(command);

            if (!string.IsNullOrWhiteSpace(result.Message))
            {
                return Result<int>.Fail(result.Message);
            }

            return Result<int>.Ok(result.CausalId);
        }

        public async Task<Result> UpdateAsync(
                int causalId,
                UpdateCausalRequest request,
                string usuario,
                CancellationToken ct)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var command = new CommandDefinition("dbo.SP_Causales_Update",
                new
                {
                    CausalId = causalId,
                    request.Nombre,
                    Usuario = usuario
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct);

            await connection.ExecuteAsync(command);

            return Result.Success();
        }

        public async Task<Result> SetActiveAsync(int causalId, bool activo, string usuario, CancellationToken ct)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var command = new CommandDefinition(
                "dbo.SP_Causales_SetActive",
                new
                {
                    CausalId = causalId,
                    Activo = activo,
                    Usuario = usuario
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct);

            await connection.ExecuteAsync(command);

            return Result.Success();
        }

        private sealed class CreateCausalResult
        {
            public int CausalId { get; set; }
            public string? Message { get; set; }
        }
    }
}
