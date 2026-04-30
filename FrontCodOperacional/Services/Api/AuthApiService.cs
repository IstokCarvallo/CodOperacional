using FrontCodOperacional.Models.Auth;
using System.Net;
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
        // 🔵 LOGIN 
        public async Task<LoginResponse?> Login(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("auth/login", request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }
        // 🔴 REFRESH TOKEN
        public async Task<LoginResponse?> Refresh(RefreshRequest request)
        {
            var response = await _http.PostAsJsonAsync("auth/refresh", request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return null;
            
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }

        // 🔴 LOGOUT
        public async Task<bool> Logout(RefreshRequest request)
        {
            var response = await _http.PostAsJsonAsync("auth/logout", request);

            return response.IsSuccessStatusCode;
        }
    }
}
