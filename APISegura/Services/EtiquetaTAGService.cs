using APISegura.Common;
using APISegura.Dtos.EtiquetasTAG;
using APISegura.Repositories.Interfaces;
using APISegura.Services.Interfaces;

namespace APISegura.Services
{
    public class EtiquetaTAGService : IEtiquetaTAGService
    {
        private readonly IEtiquetaTAGRepository _repository;
        private readonly IHttpContextAccessor _http;

        public EtiquetaTAGService(IEtiquetaTAGRepository repository,
            IHttpContextAccessor http)
        {
            _repository = repository;
            _http = http;
        }

        public Task<Result<List<EtiquetaTAGDto>>> SearchAsync(string? filtro,
        CancellationToken cancellationToken)
        {
            return _repository.SearchAsync(filtro, cancellationToken);
        }

        public Task<Result<int>> CreateAsync(
                CreateEtiquetaTAGRequest request,
                CancellationToken ct)
        {
            var user = _http.HttpContext?.User?.Identity?.Name ?? "system";

            return _repository.CreateAsync(request, user, ct);
        }
    }
}
