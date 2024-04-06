using Microsoft.AspNetCore.Mvc;
using TopStories.Common.Models;
using TopStories.Services.CacheService;
using TopStories.Services.HackerNews;

namespace TopStories.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StoriesController : ControllerBase
{
    private readonly ILogger<StoriesController> _logger;
    private readonly CacheService _cacheService;
    private readonly IApiService _hackerNewsService;

    public StoriesController(ILogger<StoriesController> logger, CacheService cacheService, IApiService hackerNewsService)
    {
        _logger = logger;
        _cacheService = cacheService;
        _hackerNewsService = hackerNewsService;
    }

    [HttpGet(Name = "GetTopStories")]
    public async Task<IEnumerable<Story>> Get(int numberOfStories)
    {
        var identifiers = _cacheService.GetOrSet<IEnumerable<int>>("BestStoriesIdentifiers", () => _hackerNewsService.GetTopStoriesIds().Result);

        if(identifiers != null)
        {
            var ids = identifiers.ToList();
            var stories = new List<Story>();
            for(int i = 0; i < numberOfStories; i++)
            {
                stories.Add(_cacheService.GetOrSet<Story>($"{ids[i]}", () => _hackerNewsService.GetStory(ids[i]).Result));
            }

            return stories;
        }

        throw new Exception();
        // List<Story> stories = new List<Story>();
        // await Task.Delay(1);
        // return stories;
    }
}
