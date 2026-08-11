using APISegura.Common;
using APISegura.Dtos.EtiquetasTAG;
using APISegura.Repositories.Interfaces;
using APISegura.Services.Interfaces;

namespace APISegura.Services
{
    public class EtiquetaTAGService : IEtiquetaTAGService
    {
        private readonly IEtiquetaTAGRepository _repository;

        public EtiquetaTAGService(IEtiquetaTAGRepository repository)
        {
            _repository = repository;
        }

        public Task<Result<List<EtiquetaTAGDto>>> SearchAsync(string? filtro,
        CancellationToken cancellationToken)
        {
            return _repository.SearchAsync(filtro, cancellationToken);
        }

        public Task<Result<int>> CreateAsync(CreateEtiquetaTAGRequest request,
        CancellationToken cancellationToken)
        {
            return _repository.CreateAsync(request, cancellationToken);
        }
    }
}
