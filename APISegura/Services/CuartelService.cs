using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Cuarteles;
using APISegura.Repositories.Interfaces;

namespace APISegura.Services
{
    public class CuartelService
    {
        private readonly ICuartelRepository _repo;
        private readonly IHttpContextAccessor _http;
        private readonly ILogger<CuartelService> _logger;

        public CuartelService(ICuartelRepository repo,
            IHttpContextAccessor http,
            ILogger<CuartelService> logger)
        {
            _repo = repo;
            _http = http;
            _logger = logger;
        }

        public async Task<List<CatalogoDto>>GetProductores(string? filtro)
        {
            try
            {
                return await _repo.GetProductoresAsync(filtro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo productores");
                throw;
            }
        }

        public async Task<List<CatalogoDto>>GetPredios(int productor, string? filtro)
        {
            if (productor <= 0)
                return new List<CatalogoDto>();

            try
            {
                return await _repo.GetPrediosAsync(productor, filtro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo predios");
                throw;
            }
        }

        public async Task<List<CuartelDto>>Search(int productor, int predio, string? filtro)
        {
            try
            {
                return await _repo.SearchAsync(productor, predio, filtro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error búsqueda cuarteles");
                throw;
            }
        }

        public async Task<Result>Update(UpdateCodigoOperacionalCuartelDto dto)
        {
            var user = _http.HttpContext?.User?.Identity?.Name ?? "system";

            try
            {
                return await _repo.UpdateCodigoOperacionalAsync(
                    dto.Productor,
                    dto.Predio,
                    dto.CodigoCuartel,
                    dto.CodigoOperacional,
                    user
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error update cuartel");
                throw;
            }
        }
    }
}
