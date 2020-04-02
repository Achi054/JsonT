using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigTranformer.Models;
using Newtonsoft.Json.Linq;

namespace ConfigTranformer.Utilities.Read
{
    public interface IReader
    {
        Task<IAsyncEnumerable<Configuration>> ReadAndDeserializeConfig(string configPath);

        Task<JObject> ReadAndDeserializeAppConfig(string file);
    }
}
