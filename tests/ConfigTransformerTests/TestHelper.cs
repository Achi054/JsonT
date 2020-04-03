using System.IO;
using Newtonsoft.Json;

namespace ConfigTransformerTests
{
    public static class TestHelper
    {
        public static void AddContent(string file, string data)
        {
            using StreamWriter fileStream = File.CreateText(file);
            using JsonTextWriter writer = new JsonTextWriter(fileStream);
            writer.WriteRaw(data);
            writer.Flush();
        }

        public static void ClearContent(string file)
            => File.WriteAllText(file, string.Empty);
    }
}
