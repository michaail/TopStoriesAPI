using TopStories.Common.Models;

namespace TopStories.Services.HackerNews;

public interface IApiService
{
    public Task<IEnumerable<int>> GetTopStoriesIds();
    public Task<Story> GetStory(int id);
}
