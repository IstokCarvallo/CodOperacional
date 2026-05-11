using APISegura.Services.Interfaces;
using Application.DTOs.Dashboard;
using Domain.Interfaces.Repositories;

namespace APISegura.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(IDashboardRepository repository,
                                ILogger<DashboardService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<DashboardUltimoCuartelDto>> ObtenerUltimosCuartelesAsync()
        {
            try
            {
                return await _repository.ObtenerUltimosCuartelesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo últimos cuarteles");
                throw;
            }
        }

        public async Task<IEnumerable<DashboardUltimaPlantaDto>> ObtenerUltimasPlantasAsync()
        {
            try
            {
                return await _repository.ObtenerUltimasPlantasAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo últimas plantas");
                throw;
            }

        }

        public async Task<IEnumerable<DashboardCajasPorCodigoDto>> ObtenerCajasPorCodigoAsync()
        {
            try
            {
                return await _repository.ObtenerCajasPorCodigoAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo cajas por código");
                throw;
            }

        }

        public async Task<IEnumerable<DashboardTotalCajasPorEspecieDto>> ObtenerTotalCajasHoyAsync()
        {
            try
            {
                return await _repository.ObtenerTotalCajasHoyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo total cajas hoy");
                throw;
            }

        }

        public async Task<int> ObtenerTotalPalletsHoyAsync()
        {
            try
            {
                return await _repository.ObtenerTotalPalletsHoyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo total pallets hoy");
                throw;
            }
        }

        public async Task<DashboardResumenDto> ObtenerResumenAsync()
        {
            try
            {
                var ultimosCuartelesTask = _repository.ObtenerUltimosCuartelesAsync();

                var ultimasPlantasTask = _repository.ObtenerUltimasPlantasAsync();

                var cajasPorCodigoTask = _repository.ObtenerCajasPorCodigoAsync();

                var totalCajasHoyTask = _repository.ObtenerTotalCajasHoyAsync();

                var totalPalletsHoyTask = _repository.ObtenerTotalPalletsHoyAsync();

                await Task.WhenAll(
                    ultimosCuartelesTask,
                    ultimasPlantasTask,
                    cajasPorCodigoTask,
                    totalCajasHoyTask,
                    totalPalletsHoyTask);

                return new DashboardResumenDto
                {
                    UltimosCuarteles = await ultimosCuartelesTask,
                    UltimasPlantas = await ultimasPlantasTask,
                    CajasPorCodigo = await cajasPorCodigoTask,
                    TotalCajasHoy = await totalCajasHoyTask,
                    TotalPalletsHoy = await totalPalletsHoyTask
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo resumen dashboard");
                throw;
            }
        }
    }
}
