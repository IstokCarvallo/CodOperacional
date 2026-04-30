using FrontCodOperacional.Models.Cuartel;
using System.Net.Http.Json;

namespace FrontCodOperacional.Services.Api
{
    public class CuartelesService
    {
        private readonly HttpClient _http;

        public CuartelesService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProductorDto>> GetProductores(string? filtro = null)
        {
            var url = "cuarteles/productores";

            if (!string.IsNullOrWhiteSpace(filtro))
                url += $"?filtro={Uri.EscapeDataString(filtro)}";

            return await _http.GetFromJsonAsync<List<ProductorDto>>(url)
                   ?? [];
        }

        public async Task<List<PredioDto>> GetPredios(int productor, string? filtro = null)
        {
            var url = $"cuarteles/predios?productor={productor}";

            if (!string.IsNullOrWhiteSpace(filtro))
                url += $"&filtro={Uri.EscapeDataString(filtro)}";

            return await _http.GetFromJsonAsync<List<PredioDto>>(url)
                   ?? [];
        }

        public async Task<List<CuartelDto>> GetCuarteles(int productor, int predio)
        {
            return await _http.GetFromJsonAsync<List<CuartelDto>>($"cuarteles?productor={productor}&predio={predio}")
                   ?? [];
        }

        public async Task UpdateCodigoOperacional(UpdateCodigoOperacionalDto dto)
        {
            var response = await _http.PostAsJsonAsync("cuarteles/codigo-operacional", dto);
            response.EnsureSuccessStatusCode();
        }
    }
}
