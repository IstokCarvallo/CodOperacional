using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Inspecciones;
using APISegura.Helpers;
using APISegura.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APISegura.Repositories
{
    public class InspeccionRepository : IInspeccionRepository
    {
        private readonly IConfiguration _config;

        public InspeccionRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Result<long>> CreateAsync(
            CreateInspeccionRequest request,
            int usuarioId,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var folios = CreateFoliosDataTable(request.Folios);
            var etiquetas = CreateEtiquetasDataTable(request.EtiquetasTAG);
            var parameters = new DynamicParameters();

            parameters.Add(
                "@FechaInspeccion",
                request.FechaInspeccion,
                DbType.DateTime2);

            parameters.Add(
                "@FechaCorreo",
                request.FechaCorreo,
                DbType.DateTime2);

            parameters.Add(
                "@NumeroCorreo",
                request.NumeroCorreo,
                DbType.String);

            parameters.Add(
                "@PateTempor",
                request.PateTempor,
                DbType.Int32);

            parameters.Add(
                "@UsuarioId",
                usuarioId,
                DbType.Int32);

            parameters.Add(
                "@Folios",
                folios.AsTableValuedParameter(
                    "dbo.InspeccionFolioType"));

            parameters.Add(
                "@EtiquetasTAG",
                etiquetas.AsTableValuedParameter(
                    "dbo.InspeccionEtiquetaTAGType"));

            var command = new CommandDefinition(
                "dbo.SP_Inspecciones_Create",
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            var inspeccionId = await connection.QuerySingleAsync<long>(
                command);

            return Result<long>.Ok(inspeccionId);
        }

        public async Task<Result<long>> UpdateAsync(
            long inspeccionId,
            CreateInspeccionRequest request,
            int usuarioId,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var folios = CreateFoliosDataTable(request.Folios);
            var etiquetas = CreateEtiquetasDataTable(request.EtiquetasTAG);

            var parameters = new DynamicParameters();

            parameters.Add(
                "@InspeccionId",
                inspeccionId,
                DbType.Int64);

            parameters.Add(
                "@FechaInspeccion",
                request.FechaInspeccion,
                DbType.DateTime2);

            parameters.Add(
                "@FechaCorreo",
                request.FechaCorreo,
                DbType.DateTime2);

            parameters.Add(
                "@NumeroCorreo",
                request.NumeroCorreo,
                DbType.String);

            parameters.Add(
                "@PateTempor",
                request.PateTempor,
                DbType.Int32);

            parameters.Add(
                "@UsuarioId",
                usuarioId,
                DbType.Int32);

            parameters.Add(
                "@Folios",
                folios.AsTableValuedParameter(
                    "dbo.InspeccionFolioType"));

            parameters.Add(
                "@EtiquetasTAG",
                etiquetas.AsTableValuedParameter(
                    "dbo.InspeccionEtiquetaTAGType"));

            var command = new CommandDefinition(
                "dbo.SP_Inspecciones_Update",
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            var result = await connection.QuerySingleAsync<long>(
                command);

            return Result<long>.Ok(result);
        }

        public async Task<Result<InspeccionDto?>> GetByIdAsync(
            long inspeccionId,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var command = new CommandDefinition(
                "dbo.SP_Inspecciones_GetById",
                new
                {
                    InspeccionId = inspeccionId
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            using var multi = await connection.QueryMultipleAsync(command);

            var inspeccion = await multi.ReadSingleOrDefaultAsync<InspeccionDto>();

            if (inspeccion is null)
            {
                return Result<InspeccionDto?>.Ok(null);
            }

            inspeccion.Folios =
                (await multi.ReadAsync<InspeccionFolioDto>())
                .AsList();

            inspeccion.EtiquetasTAG =
                (await multi.ReadAsync<InspeccionEtiquetaTAGDto>())
                .AsList();

            return Result<InspeccionDto?>.Ok(inspeccion);
        }

        public async Task<Result<PagedResult<InspeccionListDto>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            InspeccionFiltroRequest filtro,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var command = new CommandDefinition(
                "dbo.SP_Inspecciones_GetPaged",
                new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,

                    filtro.FechaInspeccionDesde,
                    filtro.FechaInspeccionHasta,

                    filtro.FechaCorreoDesde,
                    filtro.FechaCorreoHasta,

                    filtro.NumeroCorreo,
                    filtro.PateTempor,
                    filtro.InspeccionId
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            using var multi = await connection.QueryMultipleAsync(command);

            var items =
                (await multi.ReadAsync<InspeccionListDto>())
                .AsList();

            var total = await multi.ReadSingleAsync<int>();

            var result = new PagedResult<InspeccionListDto>
            {
                Items = items,
                TotalRegistros = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Result<PagedResult<InspeccionListDto>>.Ok(result);
        }

        private static DataTable CreateFoliosDataTable(
            IEnumerable<CreateInspeccionFolioRequest> folios)
        {
            var table = new DataTable();

            table.Columns.Add(
                "clie_codigo",
                typeof(decimal));

            table.Columns.Add(
                "plde_codigo",
                typeof(decimal));

            table.Columns.Add(
                "paen_numero",
                typeof(decimal));

            table.Columns.Add(
                "espe_codigo",
                typeof(decimal));

            table.Columns.Add(
                "defe_numero",
                typeof(decimal));

            table.Columns.Add(
                "nota",
                typeof(byte));

            table.Columns.Add(
                "es_supermercado",
                typeof(bool));

            table.Columns.Add(
                "causal1_id",
                typeof(int));

            table.Columns.Add(
                "causal2_id",
                typeof(int));

            table.Columns.Add(
                "causal3_id",
                typeof(int));


            table.Columns.Add(
                "observacion",
                typeof(string));

            table.Columns.Add(
                "promedio_firmeza",
                typeof(decimal));

            table.Columns.Add(
                "promedio_brix",
                typeof(decimal));

            foreach (var folio in folios)
            {
                table.Rows.Add(
                    folio.ClieCodigo,
                    folio.PldeCodigo,
                    folio.PaenNumero,
                    folio.EspeCodigo,
                    folio.DefeNumero,
                    folio.Nota,
                    folio.EsSupermercado,
                    DbValueHelper.DbValue(folio.Causal1Id),
                    DbValueHelper.DbValue(folio.Causal2Id),
                    DbValueHelper.DbValue(folio.Causal3Id),
                    DbValueHelper.DbValue(folio.Observacion),
                    DbValueHelper.DbValue(folio.PromedioFirmeza),
                    DbValueHelper.DbValue(folio.PromedioBrix));
            }

            return table;
        }

        private static DataTable CreateEtiquetasDataTable(IEnumerable<int> etiquetas)
        {
            var table = new DataTable();

            table.Columns.Add("etiqueta_id", typeof(int));

            foreach (var etiquetaId in etiquetas)
            {
                table.Rows.Add(etiquetaId);
            }

            return table;
        }
    }
}
