using LazyCache;
using Microsoft.Extensions.Logging;

namespace TopStories.Services.CacheService;

public class CacheService(ILogger<CacheService> logger, IAppCache cache) : ICacheService
{
    private readonly ILogger<CacheService> _logger = logger;
    private readonly IAppCache _cache = cache;

    /// <summary>
    /// Get data from cache using key, if data not exists in cache run valueFactory and add value to cache using key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Cache value key</param>
    /// <param name="valueFactory">Delagate to run in order to set cached value</param>
    /// <param name="expirationSpan">Expiration span for sliding expiration - absolute expiration is set to 3x sliding expiration value</param>
    /// <returns>Cached value</returns>
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
