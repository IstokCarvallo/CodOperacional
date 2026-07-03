using FrontCodOperacional.Models.Planta;
using System.Net.Http.Json;

namespace FrontCodOperacional.Services.Api
{
    public class PlantasService
    {
        private readonly HttpClient _http;

        public PlantasService(HttpClient http)
        {
            _http = http;
        }

        public async Task<PagedResult<PlantaDto>?>GetPaged(int page, int size, string? filter, CancellationToken ct)
        {
            var url = $"plantas/paged?pageNumber={page}&pageSize={size}";

            if (!string.IsNullOrWhiteSpace(filter))
                url += $"&filter={Uri.EscapeDataString(filter)}";

            return await _http.GetFromJsonAsync<PagedResult<PlantaDto>>(url, ct)
                ?? new PagedResult<PlantaDto>();
        }

        public async Task ActualizarCodigo(ActualizarCodigoOperacionalRequest request, CancellationToken ct)
        {
            var response = await _http.PostAsJsonAsync("plantas/codigo-operacional", request);
            response.EnsureSuccessStatusCode();
        }
    }
}
