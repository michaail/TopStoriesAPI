namespace TopStories.Services.CacheService;

public interface ICacheService
{
    // public T GetOrSet<T>(string key, Func<T> valueFactory);
    public Task<T> GetOrSet<T>(string key, Func<Task<T>> valueFactory);
}
