using System.Text.Json;
using Microsoft.Extensions.Logging;
using TopStories.Common.Models;
using TopStories.Services.HackerNews;

namespace TopStories.Services.HackerNewsService;

public class HackerNewsAPIService : IApiService
{
    private readonly ILogger<HackerNewsAPIService> _logger;
    private readonly  HttpClient _client;

    public HackerNewsAPIService(ILogger<HackerNewsAPIService> logger, HttpClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<Story> GetStory(int id)
    {
        _logger.LogInformation($"[HackerNewsAPI] - GET /item id: {id}");
        try
        {
            var response = await _client.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{id}.json");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<Story>(await response.Content.ReadAsStringAsync())!;
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<int>> GetTopStoriesIds()
    {
        _logger.LogInformation("[HackerNewsAPI] - GET /beststories");
        try
        {
            var response = await _client.GetAsync($"https://hacker-news.firebaseio.com/v0/beststories.json");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<IEnumerable<int>>(await response.Content.ReadAsStringAsync())!;
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
