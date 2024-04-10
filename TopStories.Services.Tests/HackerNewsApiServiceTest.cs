using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using TopStories.Common.Helpers;
using TopStories.Common.Models;
using TopStories.Services.HackerNewsService;

namespace TopStories.Tests
{
    [TestFixture]
    public class HackerNewsAPIServiceTests
    {
        private Mock<ILogger<HackerNewsAPIService>> _loggerMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private Mock<StoryConverter> _storyConverterMock;

        [SetUp]
        public void SetUp()
        {
           
            _loggerMock = new Mock<ILogger<HackerNewsAPIService>>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _storyConverterMock = new Mock<StoryConverter>();
        
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose HttpClient
        }

        [Test]
        [Ignore("httpClient")]
        public async Task GetStory_ValidId_ReturnsStory()
        {
                        var _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var _hackerNewsApiService = new HackerNewsAPIService(_loggerMock.Object, _httpClient, _storyConverterMock.Object);

            // Arrange
            var storyId = 123;
            var expectedStory = new Story { id = storyId, title = "Test Story" };
            var json = JsonSerializer.Serialize(expectedStory);
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _hackerNewsApiService.GetStory(storyId);

            // Assert
            Assert.That(result.id, Is.EqualTo(expectedStory.id));
            Assert.That(result.title, Is.EqualTo(expectedStory.title));
        }

        [Test]
        [Ignore("httpClient")]
        public async Task GetStory_InvalidId_ThrowsException()
        {
                        var _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var _hackerNewsApiService = new HackerNewsAPIService(_loggerMock.Object, _httpClient, _storyConverterMock.Object);

            // Arrange
            var storyId = -1;
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _hackerNewsApiService.GetStory(storyId));
        }

        [Test]
        public async Task GetTopStoriesIds_ReturnsIds()
        {
                                    var _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

                        var _hackerNewsApiService = new HackerNewsAPIService(_loggerMock.Object, _httpClient, _storyConverterMock.Object);

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
        [Ignore("httpClient")]
        public async Task GetTopStoriesIds_FailedRequest_ThrowsException()
        {
                        var _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var _hackerNewsApiService = new HackerNewsAPIService(_loggerMock.Object, _httpClient, _storyConverterMock.Object);

            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _hackerNewsApiService.GetTopStoriesIds());
        }
    }
}
