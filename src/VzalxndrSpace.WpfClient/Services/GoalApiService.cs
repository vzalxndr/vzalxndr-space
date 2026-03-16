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

        public GoalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<GoalDto>> GetGoalsAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", AuthService.CurrentToken);

            var response = await _httpClient.GetAsync("api/goals");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<GoalDto>>() ?? new List<GoalDto>();
            }

            return new List<GoalDto>();
        }

        public async Task<bool> CreateGoalAsync(string title, string description)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.CurrentToken);

            var content = new { Title = title, Description = description };

            var response = await _httpClient.PostAsJsonAsync("api/goals", content);

            return response.IsSuccessStatusCode;
        }

        //WARNING: endpoint doesn't exist
        //public async Task<bool> DeleteGoalAsync(Guid id)
        //{
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.CurrentToken);

        //    var response = await _httpClient.DeleteAsync($"api/goals/{id}");

        //    return response.IsSuccessStatusCode;
        //}

        public async Task<bool> CompleteGoalAsync(Guid id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.CurrentToken);

            var content = new StringContent(string.Empty);

            var response = await _httpClient.PatchAsync($"api/goals/{id}/complete", content);

            return response.IsSuccessStatusCode;
        }
    }
}
