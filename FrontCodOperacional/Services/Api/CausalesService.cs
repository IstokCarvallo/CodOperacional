using FrontCodOperacional.Models.Causal;
using FrontCodOperacional.Models.Planta;
using FrontCodOperacional.Services.Api.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace FrontCodOperacional.Services.Api
{
    public class CausalesService : ICausalesService
    {
        private readonly HttpClient _http;

        public CausalesService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CausalEspecieDto>> GetEspecies(
            string? filtro = null,
            CancellationToken cancellationToken = default)
        {
            var url = "Causales/especies";

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                url += $"?filtro={Uri.EscapeDataString(filtro)}";
            }

            var response = await _http.GetAsync(
                url,
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content.ReadFromJsonAsync<ApiResult<List<CausalEspecieDto>>>(
                    cancellationToken: cancellationToken);

            return result?.Data ?? new List<CausalEspecieDto>();
        }


        public async Task<PagedResult<CausalDto>?> GetByEspecie(
            int Codigo,
            int pageNumber,
            int pageSize,
            string? filtro = null,
            CancellationToken cancellationToken = default)
        {
            var url =
                $"Causales/especie/{Codigo}" +
                $"?pageNumber={pageNumber}" +
                $"&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                url += $"&filtro={Uri.EscapeDataString(filtro)}";
            }

            var response = await _http.GetAsync(url, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PagedResult<CausalDto>>(
                        cancellationToken: cancellationToken);

            return result;
        }


        public async Task Create(
            CreateCausalRequest request,
            CancellationToken cancellationToken = default)
        {
            var response = await _http.PostAsJsonAsync(
                "Causales",
                request,
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            response.EnsureSuccessStatusCode();
        }


        public async Task Update(
            int causalId,
            UpdateCausalRequest request,
            CancellationToken cancellationToken = default)
        {
            var response = await _http.PutAsJsonAsync(
                $"Causales/{causalId}",
                request,
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            response.EnsureSuccessStatusCode();
        }


        public async Task SetActive(
            int causalId,
            bool activo,
            CancellationToken cancellationToken = default)
        {
            var response = await _http.PatchAsJsonAsync(
                $"Causales/{causalId}/active",
                new
                {
                    Activo = activo
                },
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            response.EnsureSuccessStatusCode();
        }


        private class ApiResult<T>
        {
            public bool Success { get; set; }

            public string? Error { get; set; }

            public T? Data { get; set; }
        }
    }
}