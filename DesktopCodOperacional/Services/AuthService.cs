using DesktopCodOperacional.Models;
using DesktopCodOperacional.Models.Auth;
using DesktopCodOperacional.Services.Security;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Json;

namespace DesktopCodOperacional.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenStorageService _tokenStorage;
        private readonly SecureTokenStorageService _secureStorage;

        public AuthService(
            IHttpClientFactory factory,
            IOptions<ApiSettings> options,
            TokenStorageService tokenStorage,
            SecureTokenStorageService secureStorage)
        {
            _tokenStorage = tokenStorage;
            _secureStorage = secureStorage;
            _httpClient = factory.CreateClient();

            _httpClient.BaseAddress =
                new Uri(options.Value.BaseUrl);
        }

        public async Task<bool> LoginAsync(string usuario, string password)
        {
            try
            {
                var request = new LoginRequest
                {
                    Usuario = usuario,
                    Password = password
                };

                var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

                if (!response.IsSuccessStatusCode)
                    return false;

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result == null)
                    return false;

                _secureStorage.Save(result.AccessToken, result.RefreshToken);
                _tokenStorage.SetTokens(result.AccessToken, result.RefreshToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RefreshTokenAsync()
        {
            try
            {
                var refreshToken = _tokenStorage.GetRefreshToken();

                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    _tokenStorage.Clear();
                    return false;
                }

                var request = new RefreshRequest
                {
                    RefreshToken = refreshToken
                };

                var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", request);

                if (!response.IsSuccessStatusCode)
                {
                    _tokenStorage.Clear();
                    return false;
                }

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result == null)
                {
                    _tokenStorage.Clear();
                    return false;
                }

                _tokenStorage.SetTokens(result.AccessToken, result.RefreshToken);

                return true;
            }
            catch
            {
                _tokenStorage.Clear();
                return false;
            }
        }
        public async Task LogoutAsync()
        {
            try
            {
                await _httpClient.PostAsync("api/auth/logout", null);
            }
            catch
            {
                // opcional: ignorar errores de red
            }

            _tokenStorage.Clear();
            _secureStorage.Clear();
        }
    }
}
