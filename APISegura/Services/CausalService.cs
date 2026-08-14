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
        private readonly IHttpContextAccessor _http;

        public CausalService(
            ICausalRepository repository,
            IHttpContextAccessor http)
        {
            _repository = repository;
            _http = http;
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
            CancellationToken ct)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var (items, total) = await _repository.GetByEspecieAsync(
                Codigo, pageNumber, pageSize, filtro, ct);

            return new PagedResult<CausalDto>
            {
                Items = items,
                TotalRegistros = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public Task<Result<int>> CreateAsync(CreateCausalRequest request, CancellationToken ct)
        {
            var user = _http.HttpContext?.User?.Identity?.Name ?? "system";

            return _repository.CreateAsync(request, user, ct);
        }

        public async Task<Result> UpdateAsync(
                int causalId,
                UpdateCausalRequest request,
                CancellationToken ct)
        {
            var user = _http.HttpContext?.User?.Identity?.Name ?? "system";

            return await _repository.UpdateAsync(causalId, request, user, ct);    
        }

        public Task<Result> SetActiveAsync(int causalId, bool activo,CancellationToken ct)
        {
            var user = _http.HttpContext?.User?.Identity?.Name ?? "system"; 

            return _repository.SetActiveAsync(causalId, activo, user, ct);
        }
    }
}
