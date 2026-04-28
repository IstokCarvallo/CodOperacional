using Microsoft.JSInterop;

namespace FrontCodOperacional.Auth
{
    public class TokenStorage
    {
        private readonly IJSRuntime _js;

        private const string ACCESS_KEY = "access_token";
        private const string REFRESH_KEY = "refresh_token";

        public TokenStorage(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetToken(string token)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", ACCESS_KEY, token);
        }

        public async Task<string?> GetToken()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", ACCESS_KEY);
        }

        public async Task RemoveToken()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", ACCESS_KEY);
        }

        public async Task SetRefreshToken(string refreshToken)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", REFRESH_KEY, refreshToken);
        }

        public async Task<string?> GetRefreshToken()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", REFRESH_KEY);
        }

        public async Task RemoveRefreshToken()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", REFRESH_KEY);
        }

        public async Task SetTokens(string accessToken, string refreshToken)
        {
            await SetToken(accessToken);
            await SetRefreshToken(refreshToken);
        }

        public async Task RemoveTokens()
        {
            await RemoveToken();
            await RemoveRefreshToken();
        }
    }
}
