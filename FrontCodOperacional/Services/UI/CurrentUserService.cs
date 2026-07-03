using FrontCodOperacional.Models.User;
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

        /// <summary>
        /// Obtiene un claim por su tipo.
        /// </summary>
        public async Task<string?> GetClaim(string type)
        {
            var user = await GetUserAsync();
            return user.FindFirst(type)?.Value;
        }

        /// <summary>
        /// Id del usuario autenticado.
        /// </summary>
        public async Task<int> GetUserId()
        {
            var value = await GetClaim(ClaimTypes.NameIdentifier);

            return int.TryParse(value, out var id)
                ? id
                : 0;
        }

        /// <summary>
        /// Usuario utilizado para iniciar sesión.
        /// </summary>
        public async Task<string?> GetUsername()
        {
            var user = await GetUserAsync();
            return user.Identity?.Name;
        }

        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        public async Task<string?> GetName()
        {
            return await GetClaim("nombre");
        }

        /// <summary>
        /// Rol del usuario.
        /// </summary>
        public async Task<string?> GetRole()
        {
            return await GetClaim(ClaimTypes.Role);
        }

        /// <summary>
        /// Iniciales del nombre del usuario.
        /// </summary>
        public async Task<string> GetInitials()
        {
            var name = await GetName();

            if (string.IsNullOrWhiteSpace(name))
                return "?";

            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return parts.Length switch
            {
                0 => "?",
                1 => parts[0][0].ToString().ToUpper(),
                _ => $"{parts[0][0]}{parts[1][0]}".ToUpper()
            };
        }
        /// <summary>
        /// Obtiene toda la informacion del Usuario autenticado en un objeto CurrentUserInfo.
        /// </summary>
        public async Task<CurrentUserInfo> GetCurrentUserAsync()
        {
            var user = await GetUserAsync();

            var username = user.Identity?.Name ?? string.Empty;

            var name = user.FindFirst("nombre")?.Value ?? username;

            var role = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

            var idValue = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _ = int.TryParse(idValue, out var userId);

            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var initials = parts.Length switch
            {
                0 => "?",
                1 => parts[0][0].ToString().ToUpper(),
                _ => $"{parts[0][0]}{parts[1][0]}".ToUpper()
            };

            return new CurrentUserInfo
            {
                UserId = userId,
                Username = username,
                Name = name,
                Role = role,
                Initials = initials
            };
        }
    }
}