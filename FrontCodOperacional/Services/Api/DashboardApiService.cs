using FrontCodOperacional.Models.Dashboard;
using System.Net.Http.Json;

namespace FrontCodOperacional.Services.Api
{
    public class DashboardApiService
    {
        private readonly HttpClient _http;
        private readonly ILogger<DashboardApiService> _logger;

        public DashboardApiService(
            HttpClient http,
            ILogger<DashboardApiService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<DashboardResumenDto>ObtenerResumenAsync()
        {
            try
            {
                DashboardResumenDto? result = 
                    await _http.GetFromJsonAsync <DashboardResumenDto>("dashboard/resumen");
                return result ?? new DashboardResumenDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo resumen dashboard");
                return new DashboardResumenDto();
            }
        }

        public async Task<List<DashboardUltimoCuartelDto>>ObtenerUltimosCuartelesAsync()
        {
            try
            {
                List<DashboardUltimoCuartelDto>? result =
                    await _http.GetFromJsonAsync<List<DashboardUltimoCuartelDto>>("dashboard/ultimos-cuarteles");

                return result ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo últimos cuarteles");
                return [];
            }
        }

        public async Task<List<DashboardUltimaPlantaDto>>ObtenerUltimasPlantasAsync()
        {
            try
            {
                List<DashboardUltimaPlantaDto>? result =
                    await _http.GetFromJsonAsync<List<DashboardUltimaPlantaDto>>("dashboard/ultimas-plantas");
                return result ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo últimas plantas");
                return [];
            }
        }

        public async Task<List<DashboardCajasPorCodigoDto>>ObtenerCajasPorCodigoAsync()
        {
            try
            {
                List<DashboardCajasPorCodigoDto>? result =
                    await _http.GetFromJsonAsync<List<DashboardCajasPorCodigoDto>>("dashboard/cajas-por-codigo");
                return result ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo cajas por código");
                return [];
            }
        }

        public async Task<List<DashboardTotalCajasPorEspecieDto>>ObtenerTotalCajasHoyAsync()
        {
            try
            {
                List<DashboardTotalCajasPorEspecieDto>? result =
                    await _http.GetFromJsonAsync<List<DashboardTotalCajasPorEspecieDto>>("dashboard/total-cajas-hoy");
                return result ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo total cajas hoy");
                return [];
            }
        }

        public async Task<int>ObtenerTotalPalletsHoyAsync()
        {
            try
            {
                int result = await _http.GetFromJsonAsync<int>("dashboard/total-pallets-hoy");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo total pallets hoy");
                return 0;
            }
        }
    }
}
