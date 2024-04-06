using Microsoft.AspNetCore.Mvc;
using TopStories.Common.Models;

namespace TopStories.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StoriesController : ControllerBase
{
    private readonly ILogger<StoriesController> _logger;

    public StoriesController(ILogger<StoriesController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetTopStories")]
    public async Task<IEnumerable<Story>> Get(int numberOfStories)
    {
        List<Story> stories = new List<Story>();
        await Task.Delay(1);
        return stories;
    }
}
