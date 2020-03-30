using ConfigTranformer.Read;

namespace JsonTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            var configPath = @"C:\TigerBox\POC\JsonTransformer\JsonTConfig.json";
            var configurations = new Reader().FindAndDeserialize(configPath);
        }
    }
}
