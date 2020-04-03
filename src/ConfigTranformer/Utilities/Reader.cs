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
        public async Task<IEnumerable<Configuration>> ReadAndDeserializeConfig(string configPath)
        {
            string jsonConfigurationString = await File.ReadAllTextAsync(configPath);

            return JsonConvert.DeserializeObject<IEnumerable<Configuration>>(
                jsonConfigurationString,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
        }

        public async Task<JObject> ReadAndDeserializeAppConfig(string file)
        {
            var config = await File.ReadAllTextAsync(file);
            return string.IsNullOrWhiteSpace(config) ? null : JObject.Parse(config);
        }
    }
}
