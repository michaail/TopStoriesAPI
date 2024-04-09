using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
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
            Assert.AreEqual(expectedIdentifiers, result);
        }

        [Test]
        public async Task GetTopIdentifiers_ShouldCallApiService_WhenCacheEmpty()
        {
            // Arrange
            _cacheServiceMock.Setup(x => x.GetOrSet("BestStoriesIdentifiers", It.IsAny<Func<Task<IEnumerable<int>>>>(), It.IsAny<TimeSpan>()))
                             .ReturnsAsync((IEnumerable<int>)null);
            var expectedIdentifiers = new List<int> { 1, 2, 3 };
            _apiServiceMock.Setup(x => x.GetTopStoriesIds()).ReturnsAsync(expectedIdentifiers);

            // Act
            var result = await _topStoriesService.GetTopIdentifiers();

            // Assert
            Assert.AreEqual(expectedIdentifiers, result);
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
            Assert.AreEqual(expectedStory, result);
        }

        [Test]
        public async Task GetStory_ShouldCallApiService_WhenCacheEmpty()
        {
            // Arrange
            _cacheServiceMock.Setup(x => x.GetOrSet("123", It.IsAny<Func<Task<Story>>>(), It.IsAny<TimeSpan>()))
                             .ReturnsAsync((Story)null);
            var expectedStory = new Story { id = 123, title = "Test Story" };
            _apiServiceMock.Setup(x => x.GetStory(123)).ReturnsAsync(expectedStory);

            // Act
            var result = await _topStoriesService.GetStory(123);

            // Assert
            Assert.AreEqual(expectedStory, result);
            _apiServiceMock.Verify(x => x.GetStory(123), Times.Once);
        }
    }
}
