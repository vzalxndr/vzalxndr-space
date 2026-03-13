using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace VzalxndrSpace.WpfClient
{
    public class AuthService
    {
        private readonly HttpClient _client;

        public string? CurrentToken { get; private set; }

        public AuthService(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> LoginAsync(string password)
        {
            var request = new LoginRequest(password);

            var response = await _client.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    CurrentToken = result.Token;

                    _client.DefaultRequestHeaders.Authorization = new
                        AuthenticationHeaderValue("Bearer", CurrentToken);

                    return true;
                }
            }

            return false;
        }
    }
}
