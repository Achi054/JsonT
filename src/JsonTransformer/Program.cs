using CommandLine;
using ConfigTranformer;

namespace JsonTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonTUpdater = new ConfigurationUpdater();

            var parseResult = new Parser(with => with.HelpWriter = null)
                .ParseArguments<JsonTCommand>(args);

            parseResult
                .WithParsed(opts =>
                {
                    jsonTUpdater.UpdateConfiguration(opts.SourcePath, opts.ConfigFile).GetAwaiter().GetResult();
                })
                .WithNotParsed(errs => HelpContent.DisplayHelp(parseResult));
        }
    }
}
