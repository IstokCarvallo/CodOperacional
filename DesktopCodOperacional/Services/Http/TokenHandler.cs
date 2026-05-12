using DesktopCodOperacional.Services.Auth;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DesktopCodOperacional.Services.Http
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly TokenStorageService _tokenStorage;
        private readonly AuthService _authService;

        private static readonly SemaphoreSlim _refreshLock = new(1, 1);

        public TokenHandler(
            TokenStorageService tokenStorage,
            AuthService authService)
        {
            _tokenStorage = tokenStorage;
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            AddToken(request);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            response.Dispose();

            var refreshed = await RefreshTokenAsync();

            if (!refreshed)
            {
                _tokenStorage.Clear();
                return response;
            }

            var retryRequest = await CloneRequestAsync(request);

            AddToken(retryRequest);

            return await base.SendAsync(retryRequest, cancellationToken);
        }

        private void AddToken(HttpRequestMessage request)
        {
            var token = _tokenStorage.GetAccessToken();

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task<bool> RefreshTokenAsync()
        {
            await _refreshLock.WaitAsync();

            try
            {
                return await _authService.RefreshTokenAsync();
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            if (request.Content != null)
            {
                var content = await request.Content.ReadAsStringAsync();
                clone.Content = new StringContent(content);

                foreach (var header in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }
    }
}
