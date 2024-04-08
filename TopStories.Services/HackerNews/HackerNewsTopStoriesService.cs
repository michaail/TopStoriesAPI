using Microsoft.Extensions.Logging;
using TopStories.Common.Models;
using TopStories.Services.CacheService;

namespace TopStories.Services.HackerNews;

public class HackerNewsTopStoriesService
{
    private readonly ILogger<HackerNewsTopStoriesService> _logger;
    private readonly ICacheService _cacheService;
    private readonly IApiService _apiService;
    public HackerNewsTopStoriesService(
        ILogger<HackerNewsTopStoriesService> logger,
        ICacheService cacheService,
        IApiService apiService)
    {
        _logger = logger;
        _cacheService = cacheService;
        _apiService = apiService;
    }

    public async Task<IEnumerable<int>> GetTopIdentifiers()
    {
        return await _cacheService.GetOrSet("BestStoriesIdentifiers", async () => await _apiService.GetTopStoriesIds());
    }

    public async Task<Story> GetStory(int id)
    {
        return await _cacheService.GetOrSet($"{id}", async () => await _apiService.GetStory(id));
    }
}
