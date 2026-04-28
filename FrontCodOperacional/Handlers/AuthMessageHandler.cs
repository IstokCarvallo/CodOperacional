using FrontCodOperacional.Auth;
using FrontCodOperacional.Models.Auth;
using FrontCodOperacional.Services.Api;
using System.Net;
using System.Net.Http.Headers;

namespace FrontCodOperacional.Handlers
{
    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly CustomAuthStateProvider _auth;
        private readonly TokenStorage _storage;
        private readonly AuthApiService _authApi;
        private static readonly SemaphoreSlim _refreshLock = new(1, 1);

        public AuthMessageHandler(
            CustomAuthStateProvider auth,
            TokenStorage storage,
            AuthApiService authApi)
        {
            _auth = auth;
            _storage = storage;
            _authApi = authApi;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var path = request.RequestUri!.AbsolutePath.ToLower();

            // 🔴 1. NO interceptar endpoints de auth (evita loops)
            if (path.Contains("auth/login") ||
                path.Contains("auth/refresh") ||
                path.Contains("auth/logout"))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            // 🔵 2. Agregar access token
            var token = await _auth.GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            // 🔵 3. Ejecutar request
            var response = await base.SendAsync(request, cancellationToken);

            // 🔴 4. Si NO es 401 → salir
            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            await _refreshLock.WaitAsync(cancellationToken);

            try
            {
                // 🔵 otro request pudo haber refrescado ya
                var newToken = await _storage.GetToken();

                if (!string.IsNullOrEmpty(newToken) && newToken != token)
                {
                    var retry = await CloneHttpRequestMessageAsync(request);

                    retry.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", newToken);

                    return await base.SendAsync(retry, cancellationToken);
                }

                // 🔴 hacer refresh real
                var refreshToken = await _storage.GetRefreshToken();

                if (string.IsNullOrEmpty(refreshToken))
                {
                    await _auth.Logout();
                    return response;
                }

                var refreshResult = await _authApi.Refresh(new RefreshRequest
                {
                    RefreshToken = refreshToken
                });

                if (refreshResult == null)
                {
                    await _storage.RemoveTokens();
                    await _auth.Logout();
                    return response;
                }

                // 🔴 guardar nuevos tokens
                await _storage.SetTokens(
                    refreshResult.AccessToken,
                    refreshResult.RefreshToken);

                await _auth.SetToken(refreshResult.AccessToken);

                // 🔁 retry
                var newRequest = await CloneHttpRequestMessageAsync(request);

                newRequest.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", refreshResult.AccessToken);

                return await base.SendAsync(newRequest, cancellationToken);
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        // 🔴 necesario para reintentar requests (HttpRequestMessage no se puede reutilizar)
        private async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            // copiar headers
            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            // copiar contenido si existe
            if (request.Content != null)
            {
                var ms = new MemoryStream();
                await request.Content.CopyToAsync(ms);
                ms.Position = 0;

                clone.Content = new StreamContent(ms);

                if (request.Content.Headers != null)
                {
                    foreach (var h in request.Content.Headers)
                        clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
                }
            }

            return clone;
        }
    }
}
