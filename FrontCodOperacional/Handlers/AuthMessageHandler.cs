using FrontCodOperacional.Auth;
using FrontCodOperacional.Models.Auth;
using FrontCodOperacional.Services.Api;
using System.Net;
using System.Net.Http.Headers;

namespace FrontCodOperacional.Handlers
{
    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly TokenStorage _storage;
        private readonly AuthApiService _authApi;
        private readonly ILogger<AuthMessageHandler> _logger;

        private static readonly SemaphoreSlim _refreshLock = new(1, 1);

        public AuthMessageHandler(
            TokenStorage storage,
            AuthApiService authApi,
            ILogger<AuthMessageHandler> logger)
        {
            _storage = storage;
            _authApi = authApi;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var path = request.RequestUri?.AbsolutePath?.ToLowerInvariant() ?? "";

            if (IsAuthEndpoint(path))
            {
                _logger.LogDebug("Auth endpoint bypass: {Path}", path);
                return await base.SendAsync(request, cancellationToken);
            }

            var accessToken = await _storage.GetToken();

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);

                _logger.LogDebug("Access token attached");
            }
            else
            {
                _logger.LogWarning("No access token found");
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                _logger.LogDebug("Request OK: {Status}", response.StatusCode);
                return response;
            }

            _logger.LogWarning("401 detected → starting refresh flow");

            return await HandleUnauthorizedAsync(request, accessToken, cancellationToken);
        }

        private async Task<HttpResponseMessage> HandleUnauthorizedAsync(
            HttpRequestMessage request,
            string? originalToken,
            CancellationToken ct)
        {
            await _refreshLock.WaitAsync(ct);

            try
            {
                var latestToken = await _storage.GetToken();

                if (!string.IsNullOrWhiteSpace(latestToken) &&
                    latestToken != originalToken)
                {
                    _logger.LogInformation("Token already refreshed by another request");
                    return await Retry(request, latestToken, ct);
                }

                var refreshToken = await _storage.GetRefreshToken();

                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    _logger.LogError("No refresh token available → logout forced");
                    await _storage.RemoveTokens();
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                _logger.LogInformation("Calling refresh endpoint");

                var result = await _authApi.Refresh(new RefreshRequest
                {
                    RefreshToken = refreshToken
                });

                if (result?.AccessToken == null)
                {
                    _logger.LogError("Refresh failed → clearing session");
                    await _storage.RemoveTokens();
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                _logger.LogInformation("Refresh successful");

                await _storage.SetTokens(result.AccessToken, result.RefreshToken);

                return await Retry(request, result.AccessToken, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during refresh flow");
                await _storage.RemoveTokens();
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private async Task<HttpResponseMessage> Retry(
            HttpRequestMessage original,
            string token,
            CancellationToken ct)
        {
            _logger.LogDebug("Retrying request with new token");

            var clone = await Clone(original);

            clone.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(clone, ct);
        }

        private static async Task<HttpRequestMessage> Clone(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            foreach (var h in request.Headers)
                clone.Headers.TryAddWithoutValidation(h.Key, h.Value);

            if (request.Content != null)
            {
                var ms = new MemoryStream();
                await request.Content.CopyToAsync(ms);
                ms.Position = 0;

                clone.Content = new StreamContent(ms);

                foreach (var h in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }

            return clone;
        }

        private static bool IsAuthEndpoint(string path)
            => path.Contains("auth/login")
            || path.Contains("auth/refresh")
            || path.Contains("auth/logout");
    }
}