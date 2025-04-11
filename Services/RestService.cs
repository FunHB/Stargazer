using Stargazer.Database.Models;
using System.Diagnostics;
using System.Text.Json;

namespace Stargazer.Services
{
    public class RestService : IRestService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _serializerOptions;

        public RestService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("X-Api-Key", Constants.ApiKey);
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<T> FetchCelestialBody<T>(string name, string endpoint) where T : ICelestialBody, new()
        {
            Uri baseUri = new(string.Format(Constants.ApiUrl, string.Empty));
            Uri uri = new(baseUri, $"{endpoint}?name={name}");
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<T>>(content, _serializerOptions).FirstOrDefault() ?? new() { Name = name };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return new() { Name = name };
        }
    }
}
