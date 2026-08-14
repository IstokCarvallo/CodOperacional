using APISegura.Common;
using APISegura.Dtos.Causales;
using APISegura.Dtos.Common;

namespace APISegura.Repositories.Interfaces
{
    public interface ICausalRepository
    {
        Task<Result<List<CatalogoDto>>> GetEspeciesAsync(
                        string? filtro, CancellationToken ct);
        Task<(IEnumerable<CausalDto> Causales, int TotalRegistros)> GetByEspecieAsync(
                    int Codigo, int PageNumber, int PageSize, string? Filtro,
                    CancellationToken ct);
        Task<Result<int>> CreateAsync(
                    CreateCausalRequest request, 
                    string usuario, 
                    CancellationToken ct);

        Task<Result> UpdateAsync(
                    int causalId,
                    UpdateCausalRequest request,
                    string usuario,
                    CancellationToken ct);

        Task<Result> SetActiveAsync(
                    int causalId, 
                    bool activo,
                    string usuario,
                    CancellationToken ct);
    }
}
