using System.IO;
using System.Text.Json;

namespace ParaBankTests.Utilities
{
    public static class TestDataHelper
    {
        private static JsonElement _data;

        static TestDataHelper()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "TestData", "users.json");
            string json = File.ReadAllText(path);
            _data = JsonSerializer.Deserialize<JsonElement>(json);
        }

        public static string Get(string section, string key)
        {
            return _data.GetProperty(section).GetProperty(key).GetString() ?? string.Empty;
        }
    }
}
