using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Inspecciones;

namespace APISegura.Repositories.Interfaces
{
    public interface IInspeccionRepository
    {
        Task<Result<long>> CreateAsync(
            CreateInspeccionRequest request,
            int usuarioId,
            CancellationToken cancellationToken);

        Task<Result<InspeccionDto?>> GetByIdAsync(
            long inspeccionId,
            CancellationToken cancellationToken);

        Task<Result<PagedResult<InspeccionListDto>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? filtro,
            CancellationToken cancellationToken);
    }
}
