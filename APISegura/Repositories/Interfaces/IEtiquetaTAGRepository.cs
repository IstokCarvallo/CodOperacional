using APISegura.Common;
using APISegura.Dtos.EtiquetasTAG;

namespace APISegura.Repositories.Interfaces
{
    public interface IEtiquetaTAGRepository
    {
        Task<Result<List<EtiquetaTAGDto>>> SearchAsync(string? filtro,
        CancellationToken cancellationToken);

        Task<Result<int>> CreateAsync(CreateEtiquetaTAGRequest request,
        CancellationToken cancellationToken);
    }
}
