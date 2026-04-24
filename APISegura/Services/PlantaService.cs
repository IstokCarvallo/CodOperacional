using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Planta;
using APISegura.Entities;
using APISegura.Repositories;
using APISegura.Repositories.Interfaces;

namespace APISegura.Services
{
    public class PlantaService
    {
        private readonly IPlantaRepository _repository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<PlantaService> _logger;

        public PlantaService(
            IPlantaRepository repository,
            IHttpContextAccessor httpContext,
            ILogger<PlantaService> logger)
        {
            _repository = repository;
            _httpContext = httpContext;
            _logger = logger;
        }

        public async Task<List<PlantaDto>> Search(string? filtro)
        {
            try
            {
                return await _repository.SearchAsync(filtro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en búsqueda de plantas. Filtro: {Filtro}", filtro);
                throw;
            }
        }

        public async Task<Result> UpdateCodigoOperacional(int codigo, string nuevoCodigo)
        {
            if (string.IsNullOrWhiteSpace(nuevoCodigo))
                return Result.Failure("Código operacional inválido");

            var usuario = _httpContext.HttpContext?.User?.Identity?.Name ?? "system";

            try
            {
                return await _repository.UpdateCodigoOperacionalAsync(
                    codigo,
                    nuevoCodigo.Trim(),
                    usuario
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando código operacional. Codigo: {Codigo}, Nuevo: {NuevoCodigo}", codigo, nuevoCodigo);
                throw;
            }
        }

        public async Task<PagedResult<Planta>> GetPagedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var (items, total) = await _repository.GetPagedAsync(pageNumber, pageSize);

            return new PagedResult<Planta>
            {
                Items = items,
                TotalRegistros = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
