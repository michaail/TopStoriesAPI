using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TopStories.Common.Helpers;
using TopStories.Common.Models;
using TopStories.Services.Tests.Utils;

namespace TopStories.Services.Tests
{
    [TestFixture]
    public class StoryConverterTest
    {
        private JsonSerializerOptions? _options;

        [SetUp]
        public void SetUp()
        {
            var storyConverter = new StoryConverter();
            _options = new JsonSerializerOptions { Converters = { storyConverter } };
        }

        [Test]
        public void ConversionIsCorrect()
        {
            var storyRawData = TestDataProvider.GetTestDataString("Story");
            var expected = new Story
            {
                id = 39973467,
                postedBy = "tosh",
                commentCount = 141,
                score = 865,
                time = 1712608729,
                title = "LLM training in simple, pure C/CUDA",
                url = "https://github.com/karpathy/llm.c",
            };

            var result = JsonSerializer.Deserialize<Story>(storyRawData, _options);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
