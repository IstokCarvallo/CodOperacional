using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FrontCodOperacional.Services.UI
{
    public class CurrentUserService
    {
        private readonly AuthenticationStateProvider _auth;

        private ClaimsPrincipal? _cachedUser;

        public CurrentUserService(AuthenticationStateProvider auth)
        {
            _auth = auth;
        }

        public async Task<ClaimsPrincipal> GetUserAsync()
        {
            if (_cachedUser != null)
                return _cachedUser;

            var state = await _auth.GetAuthenticationStateAsync();
            _cachedUser = state.User;

            return _cachedUser;
        }

        public void Clear()
        {
            _cachedUser = null;
        }

        public async Task<bool> IsAuthenticated()
        {
            var user = await GetUserAsync();
            return user.Identity?.IsAuthenticated == true;
        }

        public async Task<string?> GetClaim(string type)
        {
            var user = await GetUserAsync();
            return user.FindFirst(type)?.Value;
        }

        public async Task<string?> GetName()
        {
            var user = await GetUserAsync();
            return user.Identity?.Name;
        }

        public async Task<string?> GetRole()
        {
            var user = await GetUserAsync();
            return user.FindFirst(ClaimTypes.Role)?.Value;
        }
        public async Task<string> GetInitials()
        {
            var name = await GetName();

            if (string.IsNullOrWhiteSpace(name))
                return "?";

            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
                return parts[0][0].ToString().ToUpper();

            return $"{parts[0][0]}{parts[1][0]}".ToUpper();
        }
    }
}