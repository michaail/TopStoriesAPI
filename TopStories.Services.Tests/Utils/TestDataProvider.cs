using System.Text.Json;


namespace TopStories.Services.Tests.Utils
{
    internal static class TestDataProvider
    {
        private static readonly string pathPrefix = "Resources";

        internal static T GetTestData<T>(string testDataFile)
        {
            using (StreamReader r = new($"{pathPrefix}/{testDataFile}.json"))
            {
                string data = r.ReadToEnd();
                return JsonSerializer.Deserialize<T>(data)!;
            }
        }

        internal static string GetTestDataString(string testDataFile) 
        {
            using (StreamReader r = new($"{pathPrefix}/{testDataFile}.json"))
            {
                return r.ReadToEnd();
            }
        }
    }
}
