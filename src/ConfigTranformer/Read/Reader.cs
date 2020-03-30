using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConfigTranformer.Read
{
    public class Reader : IReader
    {
        public Task<IEnumerable<Configuration>> FindAndDeserialize(string configPath)
        {
            string jsonConfigurationString = File.ReadAllText(configPath);

            var configuration = JsonConvert.DeserializeObject<IEnumerable<Configuration>>(
                jsonConfigurationString,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });

            return Task.FromResult(configuration);
        }
    }
}
