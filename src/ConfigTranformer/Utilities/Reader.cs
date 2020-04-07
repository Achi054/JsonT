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
        public Task<IEnumerable<Configuration>> ReadAndDeserializeConfig(string configPath)
        {
            string jsonConfigurationString = File.ReadAllText(configPath);

            return Task.FromResult(JsonConvert.DeserializeObject<IEnumerable<Configuration>>(
                jsonConfigurationString,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                }));
        }

        public Task<JObject> ReadAndDeserializeAppConfig(string file)
        {
            var config = File.ReadAllText(file);
            return Task.FromResult(string.IsNullOrWhiteSpace(config) ? null : JObject.Parse(config));
        }
    }
}
