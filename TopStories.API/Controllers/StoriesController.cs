using Microsoft.AspNetCore.Mvc;
using TopStories.Common.Models;
using TopStories.Services.HackerNews;

namespace TopStories.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StoriesController : ControllerBase
{
    private readonly ILogger<StoriesController> _logger;
    private readonly HackerNewsTopStoriesService _topStoriesService;

    public StoriesController(ILogger<StoriesController> logger, HackerNewsTopStoriesService topStoriesService)
    {
        _logger = logger;
        _topStoriesService = topStoriesService;
    }

    [HttpGet(Name = "TopStories")]
    public async Task<IEnumerable<Story>> Get(int numberOfStories)
    {
        // var identifiers = _cacheService.GetOrSet<IEnumerable<int>>("BestStoriesIdentifiers", () => _hackerNewsService.GetTopStoriesIds().Result);
        var identifiers = await _topStoriesService.GetTopIdentifiers();
        if(identifiers != null)
        {
            var ids = identifiers.ToList();
            var stories = new List<Story>();
            for(int i = 0; i < numberOfStories; i++)
            {
                stories.Add(await _topStoriesService.GetStory(ids[i]));
            }

            return stories;
        }

        throw new Exception();
    }
}
