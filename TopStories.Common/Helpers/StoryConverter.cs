using System.Text.Json;
using System.Text.Json.Serialization;
using TopStories.Common.Models;

namespace TopStories.Common.Helpers;

public class StoryConverter : JsonConverter<StoryResult>
    {
        public override StoryResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Deserialize the original JSON into OriginalFormat
            var original = JsonSerializer.Deserialize<Story>(ref reader, options);

            // Convert OriginalFormat to DesiredFormat
            var desired = new StoryResult
            {
                title = original.title,
                url = original.url,
                postedBy = original.by,
                time = original.time,
                score = original.score,
                commentCount = original.descendants
            };

            return desired;
        }

        public override void Write(Utf8JsonWriter writer, StoryResult value, JsonSerializerOptions options)
        {
            // Serialize DesiredFormat to JSON
            JsonSerializer.Serialize(writer, value, options);
        }
    }
