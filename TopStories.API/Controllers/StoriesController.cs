using Microsoft.AspNetCore.Mvc;
using TopStories.Common.Models;
using TopStories.Services.TopStoriesService;

namespace TopStories.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StoriesController : ControllerBase
{
    private readonly ILogger<StoriesController> _logger;
    private readonly ITopStoriesService _topStoriesService;

    public StoriesController(ILogger<StoriesController> logger, ITopStoriesService topStoriesService)
    {
        _logger = logger;
        _topStoriesService = topStoriesService;
    }

    [HttpGet(Name = "TopStories")]
    public async Task<IEnumerable<Story>> Get(int numberOfStories)
    {
        _logger.LogInformation($"Received request for: {numberOfStories} most recent stories");
        var identifiers = await _topStoriesService.GetTopIdentifiers();
        if(identifiers != null)
        {
            var tasks = new List<Task<Story>>();
            var ids = identifiers.ToList();

            for(int i = 0; i < numberOfStories; i++)
            {
                tasks.Add(_topStoriesService.GetStory(ids[i]));
            }

            return await Task.WhenAll<Story>(tasks);

        }

        throw new Exception();
    }
}
