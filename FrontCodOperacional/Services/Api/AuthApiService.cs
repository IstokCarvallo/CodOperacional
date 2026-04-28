using FrontCodOperacional.Models.Auth;
using System.Net.Http.Json;

namespace FrontCodOperacional.Services.Api
{
    public class AuthApiService
    {
        private readonly HttpClient _http;

        public AuthApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<LoginResponse?> Login(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("auth/login", request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }
    }
}
