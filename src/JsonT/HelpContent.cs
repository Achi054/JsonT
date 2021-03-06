﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace JsonT
{
    internal class HelpContent
    {
        static List<string> Help = new[] {
            "Usage: JsonT [options]",
            Environment.NewLine,
            "Options:",
            "  help, --help                  Display help",
            Environment.NewLine,
            "Command Options:",
            "  -s, --sourcepath:             Provide path to Json file(s)"
        }.ToList();

        public static void DisplayHelp<T>(ParserResult<T> result)
        {
            var versionString = Assembly.GetEntryAssembly()
                                   .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                   .InformationalVersion.ToString();

            var copyrightstring = Assembly.GetEntryAssembly()
                                .GetCustomAttribute<AssemblyCopyrightAttribute>()
                                .Copyright.ToString();

            HelpText helpText = HelpText.AutoBuild(result, h =>
            {
                h.Heading = $"JsonT v{versionString}";
                h.Copyright = copyrightstring;
                h.AutoHelp = h.AutoVersion = h.AdditionalNewLineAfterOption = false;
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);

            helpText.AddPostOptionsLines(Help);

            Console.WriteLine(helpText);
        }
    }
}
