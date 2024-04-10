using TopStories.Common;
using TopStories.Common.Models;
using TopStories.Services.CacheService;
using TopStories.Services.HackerNews;

namespace TopStories.Services.TopStoriesService;

public class HackerNewsTopStoriesService(
    ICacheService cacheService,
    IApiService apiService) : ITopStoriesService
{
    // It's slidingExpiration time - AbsoluteExpirationRelativeToNow is set to 3x slidingExpiration value
    private readonly TimeSpan identifiersExpirationSpan = TimeSpan.FromMinutes(Constants.IdentifiersCacheExpirationInMinutes);
    private readonly TimeSpan storyExpirationSpan = TimeSpan.FromMinutes(Constants.StoryCacheExpirationInMinutes);

    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(_cacheService));
    private readonly IApiService _apiService = apiService ?? throw new ArgumentNullException(nameof(_apiService));

    /// <summary>
    /// Cached service for Best stories identifiers using LazyCache
    /// </summary>
    /// <returns>Cached collection of 200 identifiers for best stories</returns>
    public async Task<IEnumerable<int>> GetTopIdentifiers()
    {
        return await _cacheService.GetOrSet("BestStoriesIdentifiers", async () => await _apiService.GetTopStoriesIds(), identifiersExpirationSpan);
    }

    /// <summary>
    /// Cached service for single story using LazyCache
    /// </summary>
    /// <returns>Cached story in a ready format</returns>
    public async Task<Story> GetStory(int id)
    {
        return await _cacheService.GetOrSet($"{id}", async () => await _apiService.GetStory(id), storyExpirationSpan);
    }
}
