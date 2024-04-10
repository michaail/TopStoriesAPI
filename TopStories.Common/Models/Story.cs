using System.Text.Json.Serialization;

namespace TopStories.Common.Models
{
    public record Story
    {
        [JsonIgnore]
        public int id { get; set; }
        public string title { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string postedBy { get; set; } = string.Empty;
        public long time { get; set; }
        public int score { get; set; }
        public int commentCount { get; set; }

        
    }

    public record StoryRaw
    {
        public int id { get; set; }
        public string title { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string by { get; set; } = string.Empty;
        public long time { get; set; }
        public int score { get; set; }
        public int descendants { get; set; }
    }
}
