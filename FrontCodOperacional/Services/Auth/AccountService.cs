using FrontCodOperacional.Models.Account;
using System.Net.Http.Json;

namespace FrontCodOperacional.Services.Auth
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _http;

        public AccountService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var response = await _http.PostAsJsonAsync("/Auth/change-password", request);

            return response.IsSuccessStatusCode;
        }
    }
}
