using FrontCodOperacional.Models.Auditoria;
using System.Net.Http.Json;

namespace FrontCodOperacional.Services.Api
{
    public class AuditoriaApiService
    {
        private readonly HttpClient _http;

        public AuditoriaApiService(HttpClient http)
        {
            _http = http;
        }
        public async Task<List<AuditoriaDto>> GetAuditoriaAsync(
            DateTime? desde = null,
            DateTime? hasta = null,
            string? usuario = null,
            string? entidad = null,
            string? accion = null)
        {
            var queryParams = new List<string>();

            if (desde.HasValue)
                queryParams.Add($"desde={Uri.EscapeDataString(desde.Value.ToString("o"))}");

            if (hasta.HasValue)
                queryParams.Add($"hasta={Uri.EscapeDataString(hasta.Value.ToString("o"))}");

            if (!string.IsNullOrWhiteSpace(usuario))
                queryParams.Add($"usuario={Uri.EscapeDataString(usuario)}");

            if (!string.IsNullOrWhiteSpace(entidad))
                queryParams.Add($"entidad={Uri.EscapeDataString(entidad)}");

            if (!string.IsNullOrWhiteSpace(accion))
                queryParams.Add($"accion={Uri.EscapeDataString(accion)}");

            var query = queryParams.Any()
                ? "?" + string.Join("&", queryParams)
                : string.Empty;

            var response = await _http.GetAsync($"auditoria{query}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<AuditoriaDto>>()
                   ?? new List<AuditoriaDto>();
        }
    }
}
