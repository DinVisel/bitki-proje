using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

namespace Bitki.Blazor.Services
{
    /// <summary>
    /// A wrapper service that adds JWT authentication to HttpClient requests
    /// </summary>
    public class AuthenticatedHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localStorage;

        public AuthenticatedHttpClient(HttpClient httpClient, ProtectedLocalStorage localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        private async Task AddAuthorizationHeader()
        {
            try
            {
                var tokenResult = await _localStorage.GetAsync<string>("authToken");
                if (tokenResult.Success && !string.IsNullOrEmpty(tokenResult.Value))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", tokenResult.Value);
                }
            }
            catch
            {
                // Ignore errors during prerendering
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            await AddAuthorizationHeader();
            return await _httpClient.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent? content)
        {
            await AddAuthorizationHeader();
            return await _httpClient.PostAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value)
        {
            await AddAuthorizationHeader();
            return await _httpClient.PostAsJsonAsync(requestUri, value);
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent? content)
        {
            await AddAuthorizationHeader();
            return await _httpClient.PutAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string requestUri, T value)
        {
            await AddAuthorizationHeader();
            return await _httpClient.PutAsJsonAsync(requestUri, value);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            await AddAuthorizationHeader();
            return await _httpClient.DeleteAsync(requestUri);
        }

        // Direct HttpClient access for complex operations
        public HttpClient Client => _httpClient;
    }
}
