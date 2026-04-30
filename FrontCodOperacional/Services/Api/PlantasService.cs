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

        public async Task<PagedResult<PlantaDto>?> GetPaged(int page, int size)
        {
            return await _http.GetFromJsonAsync<PagedResult<PlantaDto>>(
                $"plantas/paged?pageNumber={page}&pageSize={size}")
                   ?? new PagedResult<PlantaDto>();
        }

        public async Task ActualizarCodigo(ActualizarCodigoOperacionalRequest request)
        {
            var response = await _http.PostAsJsonAsync(
                "plantas/codigo-operacional",
                request);
            response.EnsureSuccessStatusCode();
        }
    }
}
