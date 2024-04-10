using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using TopStories.Common.Helpers;
using TopStories.Common.Models;
using TopStories.Services.HackerNews;

namespace TopStories.Services.HackerNewsService;

public class HackerNewsAPIService : IApiService
{
    private readonly ILogger<HackerNewsAPIService> _logger;
    private readonly HttpClient _client;
    private readonly StoryConverter _storyConverter;
    private readonly JsonSerializerOptions _serializerOptions;

    public HackerNewsAPIService(ILogger<HackerNewsAPIService> logger, HttpClient client, StoryConverter storyConverter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _storyConverter = storyConverter ?? throw new ArgumentNullException(nameof(_storyConverter));
        _serializerOptions = new JsonSerializerOptions { Converters = { _storyConverter } };
    }

    /// <summary>
    /// Get single story from hacker news API
    /// </summary>
    /// <param name="id">Identifier of a story</param>
    /// <returns>Retrieved story in a ready format</returns>
    public async Task<Story> GetStory(int id)
    {
        _logger.LogInformation($"[HackerNewsAPI] - GET /item id: {id}");
        try
        {
            var response = await _client.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{id}.json");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<Story>(await response.Content.ReadAsStringAsync(), _serializerOptions)!;
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Get collection of 200 best stories from hacker news API
    /// </summary>
    /// <returns>Collection of 200 identifiers for best stories</returns>
    public async Task<IEnumerable<int>> GetTopStoriesIds()
    {
        _logger.LogInformation("[HackerNewsAPI] - GET /beststories");
        try
        {
            var response = await _client.GetAsync($"https://hacker-news.firebaseio.com/v0/beststories.json");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<IEnumerable<int>>(await response.Content.ReadAsStringAsync(), _serializerOptions)!;
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
