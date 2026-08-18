using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Folios;
using APISegura.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APISegura.Repositories
{
    public class FolioRepository : IFolioRepository
    {
        private readonly IConfiguration _config;

        public FolioRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Result<PagedResult<FolioBusquedaDto>>> SearchAsync(
            int pageNumber,
            int pageSize,
            string? filtro,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var command = new CommandDefinition(
                "dbo.SP_BuscarFolios",
                new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Filtro = filtro
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            var items = (await connection
                .QueryAsync<FolioBusquedaDto>(command))
                .AsList();

            var total = items.Count > 0
                ? items[0].TotalRegistros
                : 0;

            var result = new PagedResult<FolioBusquedaDto>
            {
                Items = items,
                TotalRegistros = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Result<PagedResult<FolioBusquedaDto>>.Ok(result);
        }
    }
}
