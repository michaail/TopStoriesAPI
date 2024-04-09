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
        _logger = logger;
        _client = client;
    }

    public async Task<Story> GetStory(int id)
    {
        _logger.LogInformation($"[HackerNewsAPI] - GET /item id: {id}");
        var response = await _client.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{id}.json");

        if(response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<Story>(await response.Content.ReadAsStringAsync())!;
        }

        throw new Exception();
    }

    public async Task<IEnumerable<int>> GetTopStoriesIds()
    {
        _logger.LogInformation("[HackerNewsAPI] - GET /beststories");
        var response = await _client.GetAsync($"https://hacker-news.firebaseio.com/v0/beststories.json");

        if(response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<IEnumerable<int>>(await response.Content.ReadAsStringAsync())!;
        }
        
        throw new Exception();
    }
}
