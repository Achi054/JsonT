using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace JsonTransformer
{
    internal class JsonTCommand : Options
    {
        [Usage(ApplicationAlias = "JsonT")]
        public static IEnumerable<Example> Examples
        {
            get => new[] {
                    new Example("Update Json file(s)",
                        new Options
                        {
                            SourcePath = "<source-directory>"
                        })
                  };
        }
    }

    internal class Options
    {
        [Option('s', "sourcepath", Required = true, HelpText = "Provide path to Json file(s)")]
        public string SourcePath { get; set; }
    }
}
