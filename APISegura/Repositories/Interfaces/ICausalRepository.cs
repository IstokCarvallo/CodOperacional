using APISegura.Common;
using APISegura.Dtos.Causales;
using APISegura.Dtos.Common;

namespace APISegura.Repositories.Interfaces
{
    public interface ICausalRepository
    {
        Task<Result<List<CatalogoDto>>> GetEspeciesAsync(string? filtro, CancellationToken cancellationToken);
        Task<Result<List<CausalDto>>> GetByEspecieAsync(int espeCodigo, CancellationToken cancellationToken);
        Task<Result<int>> CreateAsync(CreateCausalRequest request, CancellationToken cancellationToken);
        Task<Result> SetActiveAsync(int causalId, bool activo, CancellationToken cancellationToken);
    }
}
