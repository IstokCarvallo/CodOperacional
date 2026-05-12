using DesktopCodOperacional.Models.Auth;
using DesktopCodOperacional.Models.Common;
using DesktopCodOperacional.Services.Security;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;

namespace DesktopCodOperacional.Services.Auth
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenStorageService _tokenStorage;
        private readonly SecureTokenStorageService _secureStorage;
        public string Token { get; private set; } = string.Empty;
        public string CurrentRole { get; private set; } = string.Empty;
        public string CurrentUser { get; private set; } = string.Empty;
        public int CurrentUserId { get; private set; }

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

        private void ReadTokenClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(token);

            CurrentRole = jwt.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role)
                ?.Value ?? string.Empty;

            CurrentUser = jwt.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Name)
                ?.Value ?? string.Empty;

            int.TryParse(
                jwt.Claims.FirstOrDefault(x =>
                    x.Type == ClaimTypes.NameIdentifier)?.Value,
                out int userId);

            CurrentUserId = userId;
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

                Token = result.AccessToken;
                ReadTokenClaims(Token);

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

            Token = string.Empty;
            CurrentRole = string.Empty;
            CurrentUser = string.Empty;
            CurrentUserId = 0;
            await Task.CompletedTask;
        }
    }
}
