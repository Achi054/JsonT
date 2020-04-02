using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ConfigTranformer.Utilities.Write
{
    public interface IWriter
    {
        Task<JObject> UpdateConfigurations(IDictionary<string, object> sections, JObject appSettings);

        Task WriteConfigurationsToFile(string file, JObject settings);
    }
}
