using DesktopCodOperacional.Models.Cuartel;
using System.Net.Http;
using System.Net.Http.Json;

namespace DesktopCodOperacional.Services.Api
{
        public class ApiService
        {
            private readonly HttpClient _httpClient;

            public ApiService(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public async Task<T?> GetAsync<T>(string url)
            {
                return await _httpClient.GetFromJsonAsync<T>(url);
            }

        public async Task<T?> PostAsync<T>(string url, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data);

            var content = await response.Content.ReadFromJsonAsync<T>();

            return content;
        }
    }
    }