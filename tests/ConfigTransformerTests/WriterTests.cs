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
        private readonly string configFile = @"/docs/settings.json";
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
            updatedConfig.SelectToken("Metrics.Enable").Should().BeEquivalentTo(true);
            updatedConfig.SelectToken("Metrics.Tags[0]").Should().BeEquivalentTo("test1");
            updatedConfig.SelectToken("Metrics.Tags[1]").Should().BeEquivalentTo("test2");
            updatedConfig.SelectToken("Application.Name").Should().BeEquivalentTo("Writer");
            updatedConfig.SelectToken("Logging.Enable").Should().BeEquivalentTo(true);
            updatedConfig.SelectToken("Logging.OutputType").Should().BeEquivalentTo("json");
            updatedConfig.SelectToken("Logging.Levels[0]").Should().BeEquivalentTo("Error");
            updatedConfig.SelectToken("Logging.Levels[1]").Should().BeEquivalentTo("Critical");
            updatedConfig.SelectToken("Logging.Args.Path").Should().BeEquivalentTo("/logs");
            updatedConfig.SelectToken("Logging.Args.Format").Should().BeEquivalentTo("log");
            updatedConfig.SelectToken("Auditing.Enable").Should().BeEquivalentTo(false);
        }
    }
}
