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
        public async Task<IAsyncEnumerable<Configuration>> ReadAndDeserializeConfig(string configPath)
        {
            string jsonConfigurationString = await File.ReadAllTextAsync(configPath);

            var configuration = JsonConvert.DeserializeObject<IEnumerable<Configuration>>(
                jsonConfigurationString,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });

            return configuration as IAsyncEnumerable<Configuration>;
        }

        public async Task<JObject> ReadAndDeserializeAppConfig(string file)
            => JObject.Parse(await File.ReadAllTextAsync(file));
    }
}
