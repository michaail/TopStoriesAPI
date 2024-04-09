using LazyCache;
using Microsoft.Extensions.DependencyInjection;
using TopStories.Services.CacheService;
using TopStories.Services.HackerNews;
using TopStories.Services.HackerNewsService;
using TopStories.Services.TopStoriesService;

namespace TopStories.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHackerNewsServices(this IServiceCollection services)
        {
            services.AddSingleton<IAppCache, CachingService>();
            services.AddSingleton<HttpClient>();
            services.AddScoped<ICacheService, CacheService.CacheService>();
            services.AddScoped<IApiService, HackerNewsAPIService>();
            services.AddScoped<ITopStoriesService, HackerNewsTopStoriesService>();


            return services;
        }
    }
}
