using System.Threading.Tasks;
using ConfigTranformer.Models;
using Newtonsoft.Json.Linq;

namespace ConfigTranformer.Utilities.Read
{
    public interface IReader
    {
        Task<string[]> FetchJsonTFiles(string sourcePath);

        Task<Configuration> ReadAndDeserializeConfig(string sourcePath);

        Task<JObject> ReadAndDeserializeAppConfig(string file);
    }
}
