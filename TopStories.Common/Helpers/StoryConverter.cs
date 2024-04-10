using System.Text.Json;
using System.Text.Json.Serialization;
using TopStories.Common.Models;

namespace TopStories.Common.Helpers;

public class StoryConverter : JsonConverter<Story>
    {
        public override Story Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Deserialize the original JSON into OriginalFormat
            var original = JsonSerializer.Deserialize<StoryRaw>(ref reader, options)!;

            // Convert OriginalFormat to DesiredFormat
            var desired = new Story
            {
                id = original.id,
                title = original.title,
                url = original.url,
                postedBy = original.by,
                time = original.time,
                score = original.score,
                commentCount = original.descendants
            };

            return desired;
        }

        public override void Write(Utf8JsonWriter writer, Story value, JsonSerializerOptions options)
        {
            // Serialize DesiredFormat to JSON
            JsonSerializer.Serialize(writer, value, options);
        }
    }
