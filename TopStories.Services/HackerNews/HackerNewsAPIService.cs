using System.Net.Http.Json;
using TopStories.Common.Models;
using TopStories.Services.HackerNews;

namespace TopStories.Services.HackerNewsService;

public class HackerNewsAPIService : IApiService
{
    public async Task<Story> GetStory(int id)
    {
        HttpClient client = new();
        var story = await client.GetFromJsonAsync<Story>($"https://hacker-news.firebaseio.com/v0/item/{id}.json");
        
        return story!;
    }

    public async Task<IEnumerable<int>> GetTopStoriesIds()
    {
        HttpClient client = new();
        var topStoriesIds = await client.GetFromJsonAsync<IEnumerable<int>>("https://hacker-news.firebaseio.com/v0/beststories.json");
        // Make sure it is returned or retry mechanism to be added
        return topStoriesIds ?? [] ;
    }
}
