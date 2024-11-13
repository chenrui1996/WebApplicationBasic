using System.Text.Json;
using System.Text;

namespace WebApplicationDemo.Other.MakeHttpRequest
{
    public class CustomHttpService
    {
        private readonly HttpClient _httpClient;

        public CustomHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://127.0.0.1:9000/api/");
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer your_token_here");

        }

        public async Task<string> GetDataAsync()
        {
            var response = await _httpClient.GetAsync("CustomTarget");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetDataAsync(int id)
        {
            var response = await _httpClient.GetAsync("CustomTarget" + id);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostDataAsync(object data)
        {
            var jsonData = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("CustomTarget", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PutDataAsync(int id, object data)
        {
            var jsonData = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"CustomTarget/{id}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteDataAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"CustomTarget/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
