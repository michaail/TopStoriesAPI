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
        _logger.LogInformation($"[StoriesController] - Received request for: {numberOfStories} most recent stories");
        DateTime start = DateTime.Now;
        var identifiers = await _topStoriesService.GetTopIdentifiers();
        if (identifiers != null)
        {
            var tasks = new List<Task<Story>>();
            var ids = identifiers.ToList();

            for (int i = 0; i < numberOfStories; i++)
            {
                tasks.Add(_topStoriesService.GetStory(ids[i]));
            }

            var result = await Task.WhenAll(tasks);
            var duration = TimeSpan.FromTicks(DateTime.Now.Ticks - start.Ticks);
            _logger.LogInformation($"[StoriesController] - Returned {result.Count()} in {(duration.Milliseconds == 0 ? $"{duration.TotalMicroseconds}us" : $"{ duration.Milliseconds}ms")}");
            return await Task.WhenAll<Story>(tasks);

        }

        throw new Exception();
    }
}
