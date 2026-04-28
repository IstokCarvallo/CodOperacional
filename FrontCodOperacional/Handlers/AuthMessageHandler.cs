using FrontCodOperacional.Auth;
using System.Net.Http.Headers;

namespace FrontCodOperacional.Handlers
{
    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly CustomAuthStateProvider _auth;

        public AuthMessageHandler(CustomAuthStateProvider auth)
        {
            _auth = auth;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _auth.GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
