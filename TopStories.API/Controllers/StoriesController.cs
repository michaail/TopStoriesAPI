using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TopStories.Common.Models;
using TopStories.Services.TopStoriesService;

namespace TopStories.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StoriesController(ILogger<StoriesController> logger, ITopStoriesService topStoriesService) : ControllerBase
{
    private readonly ILogger<StoriesController> _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly ITopStoriesService _topStoriesService = topStoriesService ?? throw new ArgumentNullException(nameof(_topStoriesService));

    [HttpGet(Name = "TopStories")]
    [Produces(typeof(IEnumerable<Story>))]
    public async Task<IActionResult> Get([Range(0, 200)]int numberOfStories)
    {
        _logger.LogInformation("Received request for: {numberOfStories} most recent stories", numberOfStories);
        DateTime start = DateTime.Now;
        try
        {
            var identifiers = await _topStoriesService.GetTopIdentifiers();
            var tasks = new List<Task<Story>>();
            var ids = identifiers.ToList();

            for (int i = 0; i < numberOfStories; i++)
            {
                tasks.Add(_topStoriesService.GetStory(ids[i]));
            }

            var result = await Task.WhenAll(tasks);
            var duration = TimeSpan.FromTicks(DateTime.Now.Ticks - start.Ticks);
            var logMessage = $"Returned {result.Length} in {(duration.Milliseconds == 0 ? $"{duration.TotalMicroseconds}us" : $"{duration.Milliseconds}ms")}";
            _logger.LogInformation(message: logMessage);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }
}
