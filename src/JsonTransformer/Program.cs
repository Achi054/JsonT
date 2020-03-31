using System.Threading.Tasks;
using ConfigTranformer.Read;
using ConfigTranformer.Write;

namespace JsonTransformer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configPath = @"C:\TigerBox\POC\JsonTransformer\JsonTConfig.json";
            var sourcePath = @"C:\TigerBox\POC\JsonTransformer\docs\";
            var configurations = new Reader().FindAndDeserialize(configPath).GetAwaiter().GetResult();
            foreach (var config in configurations)
            {
                await new Writer().WriteToConfig(config, sourcePath);
            }
        }
    }
}
