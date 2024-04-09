using LazyCache;
using Microsoft.Extensions.Logging;

namespace TopStories.Services.CacheService;

public class CacheService : ICacheService
{
    private readonly ILogger<CacheService> _logger;
    private readonly IAppCache _cache;

    public CacheService(ILogger<CacheService> logger, IAppCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<T> GetOrSet<T>(string key, Func<Task<T>> valueFactory)
    {
        var res = await _cache.GetOrAddAsync<T>(key, async cacheEntry => 
        {
            _logger.LogInformation($"[CacheService] - Entry with key: {cacheEntry.Key} does not exist in cache");
            return await valueFactory();
        } );
        return res!;
    }

}
