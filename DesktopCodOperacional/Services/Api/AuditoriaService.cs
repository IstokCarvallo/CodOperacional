using DesktopCodOperacional.Models.Auditoria;

namespace DesktopCodOperacional.Services.Api
{
    public class AuditoriaService
    {
        private readonly ApiService _apiService;

        public AuditoriaService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<AuditoriaDto>> ObtenerAsync()
        {
            var result = await _apiService.GetAsync<List<AuditoriaDto>>("api/Auditoria");

            return result ?? [];
        }

        public async Task<List<AuditoriaDto>> BuscarAsync(
            DateTime? desde,
            DateTime? hasta,
            string? usuario,
            string? entidad,
            string? accion)
        {
            var queryParams = new List<string>();

            if (desde.HasValue)
                queryParams.Add($"desde={desde.Value:dd.MM.yyyy}");

            if (hasta.HasValue)
                queryParams.Add($"hasta={hasta.Value:dd.MM.yyyy}");

            if (!string.IsNullOrWhiteSpace(usuario))
                queryParams.Add(
                    $"usuario={Uri.EscapeDataString(usuario)}");

            if (!string.IsNullOrWhiteSpace(entidad))
                queryParams.Add(
                    $"entidad={Uri.EscapeDataString(entidad)}");

            if (!string.IsNullOrWhiteSpace(accion))
                queryParams.Add(
                    $"accion={Uri.EscapeDataString(accion)}");

            var query = string.Join("&", queryParams);

            var url = string.IsNullOrWhiteSpace(query)
                ? "api/Auditoria"
                : $"api/Auditoria?{query}";

            var result = await _apiService.GetAsync<List<AuditoriaDto>>(url);

            return result ?? [];
        }

        public async Task<AuditoriaDto?> ObtenerPorIdAsync(int id)
        {
            return await _apiService.GetAsync<AuditoriaDto>(
                $"api/Auditoria/{id}");
        }
    }
}