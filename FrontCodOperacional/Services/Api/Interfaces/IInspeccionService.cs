using FrontCodOperacional.Models.Inspeccion;
using FrontCodOperacional.Models.Planta;

namespace FrontCodOperacional.Services.Api.Interfaces
{
    public interface IInspeccionService
    {
        //Task CreateAsync(
        //    CreateInspeccionRequest request,
        //    CancellationToken cancellationToken = default);

        //Task<Result<InspeccionDto?>> GetByIdAsync(
        //    long inspeccionId,
        //    CancellationToken cancellationToken = default);

        Task<PagedResult<InspeccionDto>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            InspeccionFiltroRequest filtro,
            CancellationToken cancellationToken = default);

        //Task UpdateAsync(
        //    long inspeccionId,
        //    UpdateInspeccionRequest request,
        //    CancellationToken cancellationToken = default);

        //Task<Result<PagedResult<FolioBusquedaDto>>> BuscarFoliosAsync(
        //    int pageNumber,
        //    int pageSize,
        //    string? filtro,
        //    CancellationToken cancellationToken = default);
    }
}
