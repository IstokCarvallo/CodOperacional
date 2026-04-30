using FrontCodOperacional.Models.Planta;
using System.Net;
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
            var response = await _http.GetAsync(
                $"plantas/paged?pageNumber={page}&pageSize={size}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<PagedResult<PlantaDto>>();
        }

        public async Task<bool> ActualizarCodigo(ActualizarCodigoOperacionalRequest request)
        {
            var response = await _http.PostAsJsonAsync(
                "plantas/codigo-operacional",
                request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            return response.IsSuccessStatusCode;
        }
    }
}
