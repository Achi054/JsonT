using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ConfigTranformer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConfigTranformer.Utilities.Read
{
    public class Reader : IReader
    {
        public Task<Configuration> ReadAndDeserializeConfig(string sourcePath)
        {
            string jsonConfigurationString = File.ReadAllText(sourcePath);
            string jtFileName = new FileInfo(sourcePath).Name;
            string configFileName = jtFileName.Substring(0, jtFileName.LastIndexOf('.'));

            if (!string.IsNullOrWhiteSpace(jsonConfigurationString))
                return Task.FromResult(
                    new Configuration(configFileName,
                    JsonConvert.DeserializeObject<IDictionary<string, object>>(
                        jsonConfigurationString,
                        new JsonSerializerSettings
                        {
                            Formatting = Formatting.Indented
                        })));

            throw new InvalidDataException($"No content in file {configFileName}");
        }

        public Task<JObject> ReadAndDeserializeAppConfig(string file)
        {
            var config = File.ReadAllText(file);
            return Task.FromResult(string.IsNullOrWhiteSpace(config) ? null : JObject.Parse(config));
        }

        public Task<string[]> FetchJsonTFiles(string sourcePath)
        {
            if (Directory.Exists(sourcePath))
            {
                return Task.FromResult(Directory.GetFiles(sourcePath, "*.jt", SearchOption.AllDirectories));
            }

            throw new DirectoryNotFoundException($"Directory {sourcePath} not found.");
        }
    }
}
