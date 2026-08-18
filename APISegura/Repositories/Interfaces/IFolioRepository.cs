using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Folios;

namespace APISegura.Repositories.Interfaces
{
    public interface IFolioRepository
    {
        Task<Result<PagedResult<FolioBusquedaDto>>> SearchAsync(
            int pageNumber,
            int pageSize,
            string? filtro,
            CancellationToken cancellationToken);
    }
}
