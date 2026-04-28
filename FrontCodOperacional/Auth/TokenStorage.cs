using Microsoft.JSInterop;

namespace FrontCodOperacional.Auth
{
    public class TokenStorage
    {
        private readonly IJSRuntime _js;

        public TokenStorage(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetToken(string token)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "access_token", token);
        }

        public async Task<string?> GetToken()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", "access_token");
        }

        public async Task RemoveToken()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "access_token");
        }
    }
}
