using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace FrontCodOperacional.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly TokenStorage _storage;

        public CustomAuthStateProvider(TokenStorage storage)
        {
            _storage = storage;
        }

        public async Task SetToken(string token)
        {
            await _storage.SetToken(token);

            var authState = await GetAuthenticationStateAsync();

            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await _storage.RemoveTokens();
            var anon = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(anon)));
        }

        public async Task<string?> GetToken()
        {
            return await _storage.GetToken();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _storage.GetToken();

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(
                    new ClaimsPrincipal(new ClaimsIdentity())
                );
            }

            var claims = ParseClaimsFromJwt(token).ToList();
                       
            var roleClaims = claims
                .Where(c =>
                    c.Type == "role" ||
                    c.Type == "roles" ||
                    c.Type.Contains("role"))
                .ToList();

            foreach (var role in roleClaims)
            {
                if (!claims.Any(c => c.Type == ClaimTypes.Role && c.Value == role.Value))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Value));
                }
            }

            if (!claims.Any(c => c.Type == ClaimTypes.Name))
            {
                var sub = claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                if (!string.IsNullOrEmpty(sub))
                {
                    claims.Add(new Claim(ClaimTypes.Name, sub));
                }
            }

            var identity = new ClaimsIdentity(claims, "jwt");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            var claims = new List<Claim>();

            foreach (var kvp in keyValuePairs)
            {
                switch (kvp.Key)
                {
                    case "sub":
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, kvp.Value.ToString()));
                        claims.Add(new Claim(ClaimTypes.Name, kvp.Value.ToString()));
                        break;

                    case "role":
                        claims.Add(new Claim(ClaimTypes.Role, kvp.Value.ToString()));
                        break;

                    default:
                        claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
                        break;
                }
            }

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
