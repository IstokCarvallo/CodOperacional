using FrontCodOperacional.Models.Planta;
using FrontCodOperacional.Models.Users;
using FrontCodOperacional.Services.Api.Interfaces;
using System.Net.Http.Json;

namespace FrontCodOperacional.Services.Api
{
    public class UsersService : IUsersService
    {
        private readonly HttpClient _http;

        public UsersService(HttpClient http)
        {
            _http = http;
        }

        public async Task<PagedResult<UserDto>?> GetPaged(
            int page,
            int size,
            string? filter,
            CancellationToken ct)
        {
            var url = $"users/paged?pageNumber={page}&pageSize={size}";

            if (!string.IsNullOrWhiteSpace(filter))
                url += $"&filtro={Uri.EscapeDataString(filter)}";

            return await _http.GetFromJsonAsync<PagedResult<UserDto>>(url, ct)
                   ?? new PagedResult<UserDto>();
        }

        public async Task<UserDetailDto?> GetById(int id, CancellationToken ct)
        {
            return await _http.GetFromJsonAsync<UserDetailDto>($"users/{id}", ct);
        }

        public async Task Update(int id, UpdateUserRequest request, CancellationToken ct)
        {
            var response = await _http.PutAsJsonAsync($"users/{id}", request, ct);

            response.EnsureSuccessStatusCode();
        }

        public async Task SetStatus(int id, SetUserStatusRequest request, CancellationToken ct)
        {
            var response = await _http.PatchAsJsonAsync( $"users/{id}/status", request, ct);

            response.EnsureSuccessStatusCode();
        }

        public async Task Create(RegisterRequest request, CancellationToken ct)
        {
            var response = await _http.PostAsJsonAsync("users", request, ct);

            response.EnsureSuccessStatusCode();
        }
    }
}