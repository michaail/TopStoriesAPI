using Microsoft.Extensions.Caching.Memory;

namespace TopStories.Services.CacheService;

public class StoriesIdentifiersCache
{
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30); // Cache expiration time


    public StoriesIdentifiersCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T GetOrSet<T>(string key, Func<T> valueFactory)
    {
        if (!_cache.TryGetValue(key, out T result))
        {
            result = valueFactory();
            _cache.Set(key, result, _cacheExpiration);
        }
        return result;
    }

}
