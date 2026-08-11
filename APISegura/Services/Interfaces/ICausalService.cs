using APISegura.Common;
using APISegura.Dtos.Causales;

namespace APISegura.Services.Interfaces
{
    public interface ICausalService
    {
        Task<Result<List<CausalDto>>> GetByEspecieAsync(int espeCodigo, CancellationToken cancellationToken);

        Task<Result<int>> CreateAsync(CreateCausalRequest request, CancellationToken cancellationToken);

        Task<Result> SetActiveAsync(int causalId, bool activo, CancellationToken cancellationToken);
    }
}
