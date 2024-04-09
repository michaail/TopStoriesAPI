using TopStories.Common;
using TopStories.Common.Models;
using TopStories.Services.CacheService;
using TopStories.Services.HackerNews;

namespace TopStories.Services.TopStoriesService;

public class HackerNewsTopStoriesService : ITopStoriesService
{
    // It's slidingExpiration time - AbsoluteExpirationRelativeToNow is set to 3x slidingExpiration value
    private readonly TimeSpan identifiersExpirationSpan = TimeSpan.FromMinutes(Constants.IdentifiersCacheExpirationInMinutes);
    private readonly TimeSpan storyExpirationSpan = TimeSpan.FromMinutes(Constants.StoryCacheExpirationInMinutes);

    private readonly ICacheService _cacheService;
    private readonly IApiService _apiService;
    public HackerNewsTopStoriesService(
        ICacheService cacheService,
        IApiService apiService)
    {
        _cacheService = cacheService;
        _apiService = apiService;
    }

    public async Task<IEnumerable<int>> GetTopIdentifiers()
    {
        return await _cacheService.GetOrSet("BestStoriesIdentifiers", async () => await _apiService.GetTopStoriesIds(), identifiersExpirationSpan);
    }

    public async Task<Story> GetStory(int id)
    {
        return await _cacheService.GetOrSet($"{id}", async () => await _apiService.GetStory(id), storyExpirationSpan);
    }
}
