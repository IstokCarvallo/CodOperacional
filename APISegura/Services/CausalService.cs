using APISegura.Common;
using APISegura.Dtos.Causales;
using APISegura.Dtos.Common;
using APISegura.Repositories.Interfaces;
using APISegura.Services.Interfaces;

namespace APISegura.Services
{
    public class CausalService : ICausalService
    {
        private readonly ICausalRepository _repository;

        public CausalService(
            ICausalRepository repository)
        {
            _repository = repository;
        }

        public Task<Result<List<CatalogoDto>>> GetEspeciesAsync(string? filtro,
                                                    CancellationToken cancellationToken)
        {
            return _repository.GetEspeciesAsync(filtro, cancellationToken);
        }

        public Task<Result<List<CausalDto>>> GetByEspecieAsync(int espeCodigo, 
                                                    CancellationToken cancellationToken)
        {
            return _repository.GetByEspecieAsync(espeCodigo, cancellationToken);
        }

        public Task<Result<int>> CreateAsync(CreateCausalRequest request, 
                                                    CancellationToken cancellationToken)
        {
            return _repository.CreateAsync(request, cancellationToken);
        }

        public Task<Result> SetActiveAsync(int causalId, bool activo,
                                                    CancellationToken cancellationToken)
        {
            return _repository.SetActiveAsync(causalId, activo, cancellationToken);
        }
    }
}
