using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using TopStories.Common.Helpers;
using TopStories.Common.Models;
using TopStories.Services.HackerNewsService;
using TopStories.Services.Tests.Utils;

namespace TopStories.Tests
{
    [TestFixture]
    public class HackerNewsAPIServiceTests
    {
        private Mock<ILogger<HackerNewsAPIService>> _loggerMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<HackerNewsAPIService>>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        }

        [Test]
        public async Task GetStory_ValidId_ReturnsStory()
        {
            var _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var _hackerNewsApiService = new HackerNewsAPIService(_loggerMock.Object, _httpClient, new StoryConverter());

            // Arrange
            var storyId = 39973467;
            var title = "Llm.c – LLM training in simple, pure C/CUDA";
            var json = TestDataProvider.GetTestDataString("Story");
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _hackerNewsApiService.GetStory(storyId);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.id, Is.EqualTo(storyId));
                Assert.That(result.title, Is.EqualTo(title));
            });
        }

        [Test]
        public void GetStory_InvalidId_ThrowsException()
        {
            var _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var _hackerNewsApiService = new HackerNewsAPIService(_loggerMock.Object, _httpClient, new StoryConverter());

            // Arrange
            var storyId = -1;
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(async () => await _hackerNewsApiService.GetStory(storyId));
        }

        [Test]
        public async Task GetTopStoriesIds_ReturnsIds()
        {
            var _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var _hackerNewsApiService = new HackerNewsAPIService(_loggerMock.Object, _httpClient, new StoryConverter());

            // Arrange
            var expectedIds = new List<int> { 1, 2, 3 };
            var json = JsonSerializer.Serialize(expectedIds);
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _hackerNewsApiService.GetTopStoriesIds();

            // Assert
            CollectionAssert.AreEqual(expectedIds, result);
        }

        [Test]
        public void GetTopStoriesIds_FailedRequest_ThrowsException()
        {
            var _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var _hackerNewsApiService = new HackerNewsAPIService(_loggerMock.Object, _httpClient, new StoryConverter());

            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(async () => await _hackerNewsApiService.GetTopStoriesIds());
        }
    }
}
