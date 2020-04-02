using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigTranformer;
using ConfigTranformer.Models;
using ConfigTranformer.Utilities.Read;
using ConfigTranformer.Utilities.Write;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ConfigTransformerTests
{
    public class ConfigurationUpdaterTests
    {
        private readonly string sourcePath = @"/docs";
        private readonly string configFile = @"/docs/JsonTConfig";
        private readonly Mock<IReader> _readerMoq;
        private readonly Mock<IWriter> _writerMoq;
        private readonly Mock<ILogger<ConfigurationUpdater>> _loggerMoq;
        private readonly ConfigurationUpdater _target;

        public ConfigurationUpdaterTests()
        {
            sourcePath = AppDomain.CurrentDomain.BaseDirectory + sourcePath;
            configFile = AppDomain.CurrentDomain.BaseDirectory + configFile;
            _readerMoq = new Mock<IReader>();
            _writerMoq = new Mock<IWriter>();
            _loggerMoq = new Mock<ILogger<ConfigurationUpdater>>();
            _target = new ConfigurationUpdater(_loggerMoq.Object, _readerMoq.Object, _writerMoq.Object);
        }

        [Fact]
        public async Task UpdateConfiguration_WithValidInputs_UpdatesAppConfig()
        {
            // Arrage
            var configurations = new List<Configuration>
            {
                new Configuration
                {
                    FileName = "settings.json",
                    Sections = new Dictionary<string, object>
                    {
                        ["dop"] = true,
                    }
                }
            } as IEnumerable<Configuration>;

            _readerMoq.Setup(x => x.ReadAndDeserializeConfig(It.IsAny<string>())).Returns(Task.FromResult(configurations));

            var settingsJObject = JObject.FromObject(new { Name = "Test" });

            _readerMoq.Setup(x => x.ReadAndDeserializeAppConfig(It.IsAny<string>())).Returns(Task.FromResult(settingsJObject));

            _writerMoq.Setup(x => x.UpdateConfigurations(It.IsAny<IDictionary<string, object>>(), It.IsAny<JObject>())).Returns(Task.FromResult(settingsJObject));

            _writerMoq.Setup(x => x.WriteConfigurationsToFile(It.IsAny<string>(), It.IsAny<JObject>())).Returns(Task.CompletedTask);

            // Act
            await _target.UpdateConfiguration(sourcePath, configFile);

            // Assert

            _loggerMoq.Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
            _readerMoq.Verify(x => x.ReadAndDeserializeConfig(It.IsAny<string>()), Times.Once);
            _readerMoq.Verify(x => x.ReadAndDeserializeAppConfig(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateConfiguration_WithValidSourcePathInvalidJsonTConfig_PerformsNothing()
        {
            // Arrange
            _readerMoq.Setup(x => x.ReadAndDeserializeConfig(It.IsAny<string>()));

            // Act
            await _target.UpdateConfiguration(sourcePath, configFile);

            // Assert
            _loggerMoq.Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Never);
        }

        [Fact]
        public async Task UpdateConfiguration_WithMissingAppConfigFile_LogMessage()
        {
            // Arrage
            var configurations = new List<Configuration>
            {
                new Configuration
                {
                    FileName = "appsettings.json",
                    Sections = new Dictionary<string, object>
                    {
                        ["dop"] = true,
                    }
                }
            } as IEnumerable<Configuration>;

            _readerMoq.Setup(x => x.ReadAndDeserializeConfig(It.IsAny<string>())).Returns(Task.FromResult(configurations));

            // Act
            await _target.UpdateConfiguration(sourcePath, configFile);

            // Assert
            _loggerMoq.Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
            _readerMoq.Verify(x => x.ReadAndDeserializeConfig(It.IsAny<string>()), Times.Once);
            _readerMoq.Verify(x => x.ReadAndDeserializeAppConfig(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UpdateConfiguration_WithValidInputs_LogError()
        {
            // Arrage
            var configurations = new List<Configuration>
            {
                new Configuration
                {
                    FileName = "settings.json",
                    Sections = new Dictionary<string, object>
                    {
                        ["dop"] = true,
                    }
                }
            } as IEnumerable<Configuration>;

            _readerMoq.Setup(x => x.ReadAndDeserializeConfig(It.IsAny<string>())).Returns(Task.FromResult(configurations));

            _readerMoq.Setup(x => x.ReadAndDeserializeAppConfig(It.IsAny<string>())).ThrowsAsync(new ArgumentException());

            // Act
            await _target.UpdateConfiguration(sourcePath, configFile);

            // Assert
            _loggerMoq.Verify(x => x.Log(LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
            _writerMoq.Verify(x => x.UpdateConfigurations(It.IsAny<IDictionary<string, object>>(), It.IsAny<JObject>()), Times.Never);
        }
    }
}
