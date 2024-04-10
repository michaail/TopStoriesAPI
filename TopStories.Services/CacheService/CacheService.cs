using LazyCache;
using Microsoft.Extensions.Logging;

namespace TopStories.Services.CacheService;

public class CacheService(ILogger<CacheService> logger, IAppCache cache) : ICacheService
{
    private readonly ILogger<CacheService> _logger = logger;
    private readonly IAppCache _cache = cache;

    public async Task<T> GetOrSet<T>(string key, Func<Task<T>> valueFactory, TimeSpan expirationSpan)
    {
        var res = await _cache.GetOrAddAsync<T>(key, async cacheEntry => 
        {
            _logger.LogInformation("Entry with key: {key} does not exist in cache.", cacheEntry.Key);
            cacheEntry.AbsoluteExpirationRelativeToNow = expirationSpan.Multiply(3);
            cacheEntry.SlidingExpiration = expirationSpan;
            return await valueFactory();
        } );
        return res!;
    }

}
