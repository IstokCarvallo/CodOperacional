using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FrontCodOperacional.Services.UI
{
    public class CurrentUserService
    {
        private readonly AuthenticationStateProvider _auth;

        public CurrentUserService(AuthenticationStateProvider auth)
        {
            _auth = auth;
        }

        public async Task<ClaimsPrincipal> GetUserAsync()
        {
            var state = await _auth.GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<string?> GetClaim(string type)
        {
            var user = await GetUserAsync();
            return user.FindFirst(type)?.Value;
        }

        public async Task<string?> GetName()
            => (await GetUserAsync()).Identity?.Name;

        public async Task<string?> GetRole()
            => (await GetUserAsync()).FindFirst(ClaimTypes.Role)?.Value;
    }
}
