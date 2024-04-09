using Castle.Core.Logging;
using Moq;
using TopStories.Services.HackerNewsService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TopStories.Common.Models;
using TopStories.Services.Tests.Utils;


namespace TopStories.Services.Tests
{
    public class Tests
    {
        private Mock<HttpClient> _httpClient;
        private ILogger<HackerNewsAPIService> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new NullLoggerFactory().CreateLogger<HackerNewsAPIService>();
            _httpClient = new Mock<HttpClient>();
        }

        [Test]
        public void TestGetStory()
        {
            Mock<HackerNewsAPIService> mock = new();
            mock.Setup(m => m.GetStory(1)).Returns(Task.FromResult<Story>(TestDataProvider.GetTestData<Story>("Story")));
            Assert.Pass();
        }

        [Test]
        public void TestTopStoriesIds()
        {

        }
    }
}