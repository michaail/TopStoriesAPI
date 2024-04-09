using TopStories.Common.Models;

namespace TopStories.Services.TopStoriesService
{
    public interface ITopStoriesService
    {
        public Task<IEnumerable<int>> GetTopIdentifiers();
        public Task<Story> GetStory(int id);

    }
}
