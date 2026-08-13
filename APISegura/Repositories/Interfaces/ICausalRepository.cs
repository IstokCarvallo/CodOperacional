using APISegura.Common;
using APISegura.Dtos.Causales;
using APISegura.Dtos.Common;

namespace APISegura.Repositories.Interfaces
{
    public interface ICausalRepository
    {
        Task<Result<List<CatalogoDto>>> GetEspeciesAsync(
                        string? filtro, 
                        CancellationToken cancellationToken);
        Task<(IEnumerable<CausalDto> Causales, int TotalRegistros)> GetByEspecieAsync(
                        int Codigo, int PageNumber, int PageSize, string? Filtro,
                        CancellationToken cancellationToken);
        Task<Result<int>> CreateAsync(
                        CreateCausalRequest request, 
                        CancellationToken cancellationToken);
        Task<Result> SetActiveAsync(
                        int causalId, 
                        bool activo, 
                        CancellationToken cancellationToken);
    }
}
