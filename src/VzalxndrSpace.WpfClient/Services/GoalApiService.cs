using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
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

        public async Task<bool> ArchiveGoalAsync(Guid id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.CurrentToken);

            var content = new StringContent(string.Empty);

            var response = await _httpClient.PatchAsync($"api/goals/{id}/archive", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CompleteGoalAsync(Guid id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.CurrentToken);

            var content = new StringContent(string.Empty);

            var response = await _httpClient.PatchAsync($"api/goals/{id}/complete", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<GoalDto?> UpdateGoalAsync(Guid id, string title, string description)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", AuthService.CurrentToken);

            var content = new { Title = title, Description = description };

            var response = await _httpClient.PutAsJsonAsync($"api/goals/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<GoalDto>();
            }

            return null;
        }

        public async Task<Guid?> StartSessionAsync(Guid goalId, int targetDurationMinutes)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.CurrentToken);

            var content = new { GoalId = goalId, TargetDurationMinutes = targetDurationMinutes };
            var response = await _httpClient.PostAsJsonAsync("api/Sessions/start", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<SessionResponse>();
                return result?.SessionId;
            }
            else 
            { 
                return null;
            }
        }

        public async Task<bool> StopSessionAsync(Guid sessionId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.CurrentToken);

            var response = await _httpClient.PostAsync($"api/Sessions/{sessionId}/stop", null);

            return response.IsSuccessStatusCode;
        }
    }
}
