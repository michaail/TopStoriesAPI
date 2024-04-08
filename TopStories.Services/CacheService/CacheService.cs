using Microsoft.Extensions.Caching.Memory;

namespace TopStories.Services.CacheService;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<T> GetOrSet<T>(string key, Func<Task<T>> valueFactory)
    {
        var res = await _cache.GetOrCreateAsync<T>(key, async cacheEntry => await valueFactory());
        return res!;
    }

}
