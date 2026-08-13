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

        public async Task<PagedResult<CausalDto>> GetByEspecieAsync(
            int Codigo,
            int pageNumber,
            int pageSize,
            string? filtro,
            CancellationToken cancellationToken)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var (items, total) = await _repository.GetByEspecieAsync(
                Codigo, pageNumber, pageSize, filtro, cancellationToken);

            return new PagedResult<CausalDto>
            {
                Items = items,
                TotalRegistros = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
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
