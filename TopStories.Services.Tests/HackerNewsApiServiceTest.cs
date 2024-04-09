using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TopStories.Common.Models;
using TopStories.Services.HackerNewsService;

namespace TopStories.Tests
{
    [TestFixture]
    public class HackerNewsAPIServiceTests
    {
        private HackerNewsAPIService _hackerNewsApiService;
        private Mock<ILogger<HackerNewsAPIService>> _loggerMock;
        private Mock<HttpClient> _httpClientMock;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<HackerNewsAPIService>>();
            _httpClientMock = new Mock<HttpClient>();

            _hackerNewsApiService = new HackerNewsAPIService(_loggerMock.Object, _httpClientMock.Object);
        }

        [Test]
        public async Task GetStory_ValidId_ReturnsStory()
        {
            // Arrange
            var storyId = 123;
            var expectedStory = new Story { id = storyId, title = "Test Story" };
            var json = JsonSerializer.Serialize(expectedStory);
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
            _httpClientMock
                .Setup(client => client.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _hackerNewsApiService.GetStory(storyId);

            // Assert
            Assert.AreEqual(expectedStory.id, result.id);
            Assert.AreEqual(expectedStory.title, result.title);
        }

        [Test]
        public async Task GetStory_InvalidId_ThrowsException()
        {
            // Arrange
            var storyId = -1;
            _httpClientMock
                .Setup(client => client.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _hackerNewsApiService.GetStory(storyId));
        }

        [Test]
        public async Task GetTopStoriesIds_ReturnsIds()
        {
            // Arrange
            var expectedIds = new List<int> { 1, 2, 3 };
            var json = JsonSerializer.Serialize(expectedIds);
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
            _httpClientMock
                .Setup(client => client.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _hackerNewsApiService.GetTopStoriesIds();

            // Assert
            CollectionAssert.AreEqual(expectedIds, result);
        }

        [Test]
        public async Task GetTopStoriesIds_FailedRequest_ThrowsException()
        {
            // Arrange
            _httpClientMock
                .Setup(client => client.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _hackerNewsApiService.GetTopStoriesIds());
        }
    }
}
