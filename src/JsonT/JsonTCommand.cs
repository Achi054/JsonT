using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace JsonT
{
    internal class JsonTCommand : Options
    {
        [Usage(ApplicationAlias = "JsonT")]
        public static IEnumerable<Example> Examples
        {
            get => new[] {
                    new Example("Update application configuration files",
                        new Options
                        {
                            SourcePath = "<source-directory>",
                            ConfigFile = "<config-file>"
                        })
                  };
        }
    }

    internal class Options
    {
        [Option('s', "sourcepath", Required = true, HelpText = "Provide the source path to the config file(s)")]
        public string SourcePath { get; set; }

        [Option('c', "configfile", Required = true, HelpText = "Provide full path to JsonT config file")]
        public string ConfigFile { get; set; }
    }
}
