using Moq;
using TopStories.Common.Models;
using TopStories.Services.CacheService;
using TopStories.Services.HackerNews;

namespace TopStories.Services.TopStoriesService.Tests
{
    [TestFixture]
    public class HackerNewsTopStoriesServiceTests
    {
        private HackerNewsTopStoriesService _topStoriesService;
        private Mock<ICacheService> _cacheServiceMock;
        private Mock<IApiService> _apiServiceMock;

        [SetUp]
        public void SetUp()
        {
            _cacheServiceMock = new Mock<ICacheService>();
            _apiServiceMock = new Mock<IApiService>();
            _topStoriesService = new HackerNewsTopStoriesService(_cacheServiceMock.Object, _apiServiceMock.Object);
        }

        [Test]
        public async Task GetTopIdentifiers_ShouldReturnCachedIdentifiers()
        {
            // Arrange
            var expectedIdentifiers = new List<int> { 1, 2, 3 };
            _cacheServiceMock.Setup(x => x.GetOrSet("BestStoriesIdentifiers", It.IsAny<Func<Task<IEnumerable<int>>>>(), It.IsAny<TimeSpan>()))
                             .ReturnsAsync(expectedIdentifiers);

            // Act
            var result = await _topStoriesService.GetTopIdentifiers();

            // Assert
            Assert.That(result, Is.EqualTo(expectedIdentifiers));
        }

        [Test]
        public async Task GetTopIdentifiers_ShouldCallApiService_WhenCacheEmpty()
        {
            // Arrange
            var expectedIdentifiers = new List<int> { 1, 2, 3 };
            _apiServiceMock.Setup(x => x.GetTopStoriesIds()).ReturnsAsync(expectedIdentifiers);
            _cacheServiceMock.Setup(x => x.GetOrSet("BestStoriesIdentifiers", It.IsAny<Func<Task<IEnumerable<int>>>>(), It.IsAny<TimeSpan>()))
                             .ReturnsAsync(_apiServiceMock.Object.GetTopStoriesIds().Result);

            // Act
            var result = await _topStoriesService.GetTopIdentifiers().ConfigureAwait(false);

            // Assert
            Assert.That(result, Is.EqualTo(expectedIdentifiers));
            _apiServiceMock.Verify(x => x.GetTopStoriesIds(), Times.Once);
        }

        [Test]
        public async Task GetStory_ShouldReturnCachedStory()
        {
            // Arrange
            var expectedStory = new Story { id = 123, title = "Test Story" };
            _cacheServiceMock.Setup(x => x.GetOrSet("123", It.IsAny<Func<Task<Story>>>(), It.IsAny<TimeSpan>()))
                             .ReturnsAsync(expectedStory);

            // Act
            var result = await _topStoriesService.GetStory(123);

            // Assert
            Assert.That(result, Is.EqualTo(expectedStory));
        }

        [Test]
        public async Task GetStory_ShouldCallApiService_WhenCacheEmpty()
        {
            // Arrange
            var expectedStory = new Story { id = 123, title = "Test Story" };
            _apiServiceMock.Setup(x => x.GetStory(123)).ReturnsAsync(expectedStory);
            _cacheServiceMock.Setup(x => x.GetOrSet("123", It.IsAny<Func<Task<Story>>>(), It.IsAny<TimeSpan>()))
                             .ReturnsAsync(_apiServiceMock.Object.GetStory(123).Result);

            // Act
            var result = await _topStoriesService.GetStory(123);

            // Assert
            Assert.That(result, Is.EqualTo(expectedStory));
            _apiServiceMock.Verify(x => x.GetStory(123), Times.Once);
        }
    }
}
