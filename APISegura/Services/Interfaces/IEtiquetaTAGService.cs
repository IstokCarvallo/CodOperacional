using APISegura.Common;
using APISegura.Dtos.EtiquetasTAG;

namespace APISegura.Services.Interfaces
{
    public interface IEtiquetaTAGService
    {
        Task<Result<List<EtiquetaTAGDto>>> SearchAsync(
                string? filtro,
                CancellationToken ct);

        Task<Result<int>> CreateAsync(
                CreateEtiquetaTAGRequest request,
                CancellationToken ct);
    }
}
