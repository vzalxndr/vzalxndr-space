using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VzalxndrSpace.WpfClient.Models;

namespace VzalxndrSpace.WpfClient.Services
{
    public class GoalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public GoalApiService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        public async Task<List<GoalDto>> GetGoalsAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _authService.CurrentToken);

            var response = await _httpClient.GetAsync("api/goals");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<GoalDto>>() ?? new List<GoalDto>();
            }

            return new List<GoalDto>();
        }
    }
}
