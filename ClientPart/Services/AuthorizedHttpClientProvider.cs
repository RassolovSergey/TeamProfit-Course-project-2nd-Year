using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ClientPart.Services
{
    public class AuthorizedHttpClientProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ProtectedSessionStorage _sessionStorage;

        public AuthorizedHttpClientProvider(IHttpClientFactory httpClientFactory, ProtectedSessionStorage sessionStorage)
        {
            _httpClientFactory = httpClientFactory;
            _sessionStorage = sessionStorage;
        }

        public async Task<HttpClient> CreateClientAsync()
        {
            var client = _httpClientFactory.CreateClient("WebApi");
            var result = await _sessionStorage.GetAsync<string>("authToken");
            if (result.Success && !string.IsNullOrEmpty(result.Value))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Value);
            return client;
        }
    }
}
