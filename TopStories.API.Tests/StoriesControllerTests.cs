using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TopStories.Common.Models;
using TopStories.Services.TopStoriesService;

namespace TopStories.API.Controllers.Tests
{
    [TestFixture]
    public class StoriesControllerTests
    {
        private StoriesController _controller;
        private Mock<ILogger<StoriesController>> _loggerMock;
        private Mock<ITopStoriesService> _topStoriesServiceMock;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<StoriesController>>();
            _topStoriesServiceMock = new Mock<ITopStoriesService>();
            _controller = new StoriesController(_loggerMock.Object, _topStoriesServiceMock.Object);
        }

        [Test]
        public async Task Get_ShouldReturnOkResult_WithStories()
        {
            // Arrange
            var numberOfStories = 3;
            var identifiers = new List<int> { 1, 2, 3 };
            var stories = identifiers.Select(id => new Story { id = id, title = $"Story {id}" });
            _topStoriesServiceMock.Setup(x => x.GetTopIdentifiers()).ReturnsAsync(identifiers);
            _topStoriesServiceMock.SetupSequence(x => x.GetStory(It.IsAny<int>()))
                                  .ReturnsAsync(new Story { id = 1, title = "Story 1" })
                                  .ReturnsAsync(new Story { id = 2, title = "Story 2" })
                                  .ReturnsAsync(new Story { id = 3, title = "Story 3" });

            // Act
            var result = await _controller.Get(numberOfStories) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsInstanceOf<IEnumerable<Story>>(result.Value);
            var returnedStories = result.Value as IEnumerable<Story>;
            Assert.IsNotNull(returnedStories);
            Assert.AreEqual(numberOfStories, returnedStories.Count());
            CollectionAssert.AreEqual(stories, returnedStories);
        }

        [Test]
        public async Task Get_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var numberOfStories = 3;
            _topStoriesServiceMock.Setup(x => x.GetTopIdentifiers()).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Get(numberOfStories) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}
