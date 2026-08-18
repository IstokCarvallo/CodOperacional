using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Folios;

namespace APISegura.Services.Interfaces
{
    public interface IFolioService
    {
        Task<Result<PagedResult<FolioBusquedaDto>>> SearchAsync(
            int pageNumber,
            int pageSize,
            string? filtro,
            CancellationToken cancellationToken);
    }
}
