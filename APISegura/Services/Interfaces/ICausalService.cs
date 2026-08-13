using APISegura.Common;
using APISegura.Dtos.Causales;
using APISegura.Dtos.Common;

namespace APISegura.Services.Interfaces
{
    public interface ICausalService
    {
        Task<Result<List<CatalogoDto>>> GetEspeciesAsync(string? filtro,
                                CancellationToken cancellationToken);
        Task<PagedResult<CausalDto>> GetByEspecieAsync(
            int Codigo,
            int pageNumber,
            int pageSize,
            string? filtro,
            CancellationToken cancellationToken);
        Task<Result<int>> CreateAsync(CreateCausalRequest request,
                                CancellationToken cancellationToken);
        Task<Result> SetActiveAsync(int causalId, bool activo, 
                                CancellationToken cancellationToken);
    }
}
