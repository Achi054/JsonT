﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConfigTranformer.Utilities.Read;
using ConfigTranformer.Utilities.Write;
using Microsoft.Extensions.Logging;

namespace ConfigTranformer
{
    public class ConfigurationUpdater : IConfigurationUpdater
    {
        private readonly ILogger<ConfigurationUpdater> _logger;
        private readonly IReader _reader;
        private readonly IWriter _writer;

        public ConfigurationUpdater(ILogger<ConfigurationUpdater> logger = default)
            => (_logger, _reader, _writer) = (logger ?? GetLogger(), new Reader(), new Writer());

        public ConfigurationUpdater(ILogger<ConfigurationUpdater> logger, IReader reader, IWriter writer)
        => (_logger, _reader, _writer) = (logger ?? GetLogger(), reader, writer);

        /// <summary>
        /// Update configuration files with settings in JsonT configuration file
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="JsonTConfig"></param>
        /// <returns></returns>
        public async Task UpdateConfiguration(string sourcePath)
        {
            try
            {
                var jtFiles = await _reader.FetchJsonTFiles(sourcePath);

                foreach (var jtFile in jtFiles)
                {
                    var config = await _reader.ReadAndDeserializeConfig(jtFile);
                    var appConfigurationFile = Directory.GetFiles(sourcePath, config.FileName, SearchOption.AllDirectories)?.FirstOrDefault();
                    if (appConfigurationFile == null)
                        _logger.LogInformation($"File {config.FileName} does not exists in path {sourcePath}");
                    else
                    {
                        var settings = await _reader.ReadAndDeserializeAppConfig(appConfigurationFile);
                        var updatedConfigurations = await _writer.UpdateConfigurations(config.Sections, settings);
                        await _writer.WriteConfigurationsToFile(appConfigurationFile, updatedConfigurations);
                        _logger.LogInformation($"Updated {config.FileName} with new settings.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Updating configuration failed.\n");
            }
        }

        private static ILogger<ConfigurationUpdater> GetLogger()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("ConfigTranformer.ConfigurationUpdater", LogLevel.Trace)
                    .AddConsole();
            });

            return loggerFactory.CreateLogger<ConfigurationUpdater>();
        }
    }
}
