using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigTranformer.Utilities.Write;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ConfigTransformerTests
{
    public class WriterTests
    {
        private readonly string configFile = @"docs\settings.json";
        private readonly IWriter _target;

        public WriterTests()
        {
            configFile = AppDomain.CurrentDomain.BaseDirectory + configFile;
            _target = new Writer();
        }

        ~WriterTests() => TestHelper.ClearContent(configFile);

        [Fact]
        public async Task UpdateConfigurations_WhenValidDataIsPassed_ReturnsUpdatedJson()
        {
            // Arrange
            var appsettingsObject = new JObject
            {
                { "Logging", new JObject(new JProperty("Enable", true)) },
                { "Auditing", new JObject(new JProperty("Enable", true)) },
            };

            var section = new Dictionary<string, object>
            {
                ["Metrics"] = new JObject { ["Enable"] = true, ["Tags"] = new JArray("test1", "test2") },
                ["Application:Name"] = "Writer",
                ["Logging:OutputType"] = "json",
                ["Logging:Levels"] = new JArray("Error", "Critical"),
                ["Logging:Args"] = new JObject { ["Path"] = "/logs", ["Format"] = "log" },
                ["Auditing:Enable"] = false
            };

            // Act
            var updatedConfig = await _target.UpdateConfigurations(section, appsettingsObject);

            // Assert
            updatedConfig.Should().NotBeNull();
            updatedConfig.Should().NotBeEmpty();
            updatedConfig.SelectToken("Metrics.Enable").Value<Boolean>().Should().BeTrue();
            updatedConfig.SelectToken("Metrics.Tags[0]").Value<string>().Should().BeEquivalentTo("test1");
            updatedConfig.SelectToken("Metrics.Tags[1]").Value<string>().Should().BeEquivalentTo("test2");
            updatedConfig.SelectToken("Application.Name").Value<string>().Should().BeEquivalentTo("Writer");
            updatedConfig.SelectToken("Logging.Enable").Value<Boolean>().Should().BeTrue();
            updatedConfig.SelectToken("Logging.OutputType").Value<string>().Should().BeEquivalentTo("json");
            updatedConfig.SelectToken("Logging.Levels[0]").Value<string>().Should().BeEquivalentTo("Error");
            updatedConfig.SelectToken("Logging.Levels[1]").Value<string>().Should().BeEquivalentTo("Critical");
            updatedConfig.SelectToken("Logging.Args.Path").Value<string>().Should().BeEquivalentTo("/logs");
            updatedConfig.SelectToken("Logging.Args.Format").Value<string>().Should().BeEquivalentTo("log");
            updatedConfig.SelectToken("Auditing.Enable").Value<Boolean>().Should().BeFalse();
        }

        [Fact]
        public async Task UpdateConfigurations_WhenNoSections_ReturnsOriginalJson()
        {
            // Arrange
            var appsettingsObject = new JObject
            {
                { "Logging", new JObject(new JProperty("Enable", true)) },
                { "Auditing", new JObject(new JProperty("Enable", true)) },
            };

            var section = new Dictionary<string, object>();

            // Act
            var updatedConfig = await _target.UpdateConfigurations(section, appsettingsObject);

            // Assert
            updatedConfig.Should().NotBeNull();
            updatedConfig.Should().NotBeEmpty();
            updatedConfig.SelectToken("Logging.Enable").Value<Boolean>().Should().BeTrue();
            updatedConfig.SelectToken("Auditing.Enable").Value<Boolean>().Should().BeTrue();
        }

        [Fact]
        public async Task UpdateConfigurations_WhenNull_ReturnsOriginalJson()
        {
            // Arrange
            var appsettingsObject = new JObject
            {
                { "Logging", new JObject(new JProperty("Enable", true)) },
                { "Auditing", new JObject(new JProperty("Enable", true)) },
            };

            IDictionary<string, object> section = null;

            // Act
            var updatedConfig = await _target.UpdateConfigurations(section, appsettingsObject);

            // Assert
            updatedConfig.Should().NotBeNull();
            updatedConfig.Should().NotBeEmpty();
            updatedConfig.SelectToken("Logging.Enable").Value<Boolean>().Should().BeTrue();
            updatedConfig.SelectToken("Auditing.Enable").Value<Boolean>().Should().BeTrue();
        }

        [Fact]
        public async Task UpdateConfigurations_WhenAppSettingsIsEmpty_ReturnsAddedJson()
        {
            // Arrange
            var appsettingsObject = new JObject();

            var section = new Dictionary<string, object>
            {
                ["Metrics"] = new JObject { ["Enable"] = true }
            };

            // Act
            var updatedConfig = await _target.UpdateConfigurations(section, appsettingsObject);

            // Assert
            updatedConfig.Should().NotBeNull();
            updatedConfig.Should().NotBeEmpty();
            updatedConfig.SelectToken("Metrics.Enable").Value<Boolean>().Should().BeTrue();
        }
    }
}
