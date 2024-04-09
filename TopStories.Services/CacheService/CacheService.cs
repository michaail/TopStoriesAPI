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

    public async Task<T> GetOrSet<T>(string key, Func<Task<T>> valueFactory, TimeSpan expirationSpan)
    {
        var res = await _cache.GetOrAddAsync<T>(key, async cacheEntry => 
        {
            _logger.LogInformation($"[CacheService] - Entry with key: {cacheEntry.Key} does not exist in cache");
            cacheEntry.AbsoluteExpirationRelativeToNow = expirationSpan.Multiply(3);
            cacheEntry.SlidingExpiration = expirationSpan;
            return await valueFactory();
        } );
        return res!;
    }

}
