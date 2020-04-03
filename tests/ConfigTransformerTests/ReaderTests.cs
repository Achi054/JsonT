using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConfigTranformer.Utilities.Read;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ConfigTransformerTests
{
    public class ReaderTests
    {
        private readonly string configFile = @"/docs/JsonTConfig.json";
        private readonly IReader _target;

        public ReaderTests()
        {
            configFile = AppDomain.CurrentDomain.BaseDirectory + configFile;
            _target = new Reader();
        }

        ~ReaderTests() => TestHelper.ClearContent(configFile);

        [Fact]
        public async Task ReadAndDeserializeConfig_WhenConfigIsValid_ReturnJObject()
        {
            // Arrange
            var jappsettingsObject = new JObject
            {
                { "FileName", "appsettings.json" },
                { "Sections", new JObject
                    {
                        ["Logging:Enable"] = true,
                        ["Auditing:Enable"] = true
                    }
                }
            };

            var jappconfigsObject = new JObject
            {
                { "FileName", "appconfigs.json" },
                { "Sections", new JObject
                    {
                        ["Logging:Enable"] = false,
                        ["Auditing:Enable"] = false
                    }
                }
            };

            var jtArray = new JArray
            {
                jappsettingsObject,
                jappconfigsObject
            };

            TestHelper.AddContent(configFile, jtArray.ToString());

            // Act
            var data = await _target.ReadAndDeserializeConfig(configFile);

            // Assert
            data.Should().NotBeNull();
            data.Should().HaveCount(2);

            data.First().FileName.Should().Be("appsettings.json");
            data.First().Sections.Should().NotBeNull();
            data.First().Sections.Should().HaveCount(2);
            data.First().Sections.First().Key.Should().Be("Logging:Enable");
            data.First().Sections.First().Value.Should().Be(true);
            data.First().Sections.Skip(1).First().Key.Should().Be("Auditing:Enable");
            data.First().Sections.Skip(1).First().Value.Should().Be(true);

            data.Skip(1).First().FileName.Should().Be("appconfigs.json");
            data.Skip(1).First().Sections.Should().NotBeNull();
            data.Skip(1).First().Sections.Should().HaveCount(2);
            data.Skip(1).First().Sections.First().Key.Should().Be("Logging:Enable");
            data.Skip(1).First().Sections.First().Value.Should().Be(false);
            data.Skip(1).First().Sections.Skip(1).First().Key.Should().Be("Auditing:Enable");
            data.Skip(1).First().Sections.Skip(1).First().Value.Should().Be(false);
        }

        [Fact]
        public async Task ReadAndDeserializeConfig_WhenFileDoesntExist_ThrowsException()
        {
            // Arrange
            var file = AppDomain.CurrentDomain.BaseDirectory + "/docs/setting.json";

            // Act and Assert
            await Assert.ThrowsAsync<FileNotFoundException>(async () => await _target.ReadAndDeserializeConfig(file));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ReadAndDeserializeConfig_WhenFileExistWithNoConfigurations_ReturnNull(string configData)
        {
            // Arrange
            TestHelper.AddContent(configFile, configData);

            // Act
            var data = await _target.ReadAndDeserializeConfig(configFile);

            // Assert
            data.Should().BeNull();
        }

        [Fact]
        public async Task ReadAndDeserializeAppConfig_WhenReadingFromConfiguration_ReturnJson()
        {
            // Arrange
            var jappsettingsObject = new JObject
            {
                { "FileName", "appsettings.json" },
                { "Sections", new JObject
                    {
                        ["Logging:Enable"] = true,
                        ["Auditing:Enable"] = true
                    }
                }
            };

            TestHelper.AddContent(configFile, jappsettingsObject.ToString());

            // Act
            var data = await _target.ReadAndDeserializeAppConfig(configFile);

            // Assert
            data.Should().NotBeNull();
            data.Should().BeEquivalentTo(jappsettingsObject);
        }

        [Fact]
        public async Task ReadAndDeserializeAppConfig_WhenConfigFileDoesntExist_ThrowsException()
        {
            // Arrange
            var file = AppDomain.CurrentDomain.BaseDirectory + "/docs/setting.json";

            // Act and Assert
            await Assert.ThrowsAsync<FileNotFoundException>(async () => await _target.ReadAndDeserializeAppConfig(file));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ReadAndDeserializeAppConfig_WhenConfigContainsNoContent_ReturnNull(string configData)
        {
            // Arrange
            TestHelper.AddContent(configFile, configData);

            // Act
            var data = await _target.ReadAndDeserializeAppConfig(configFile);

            // Assert
            data.Should().BeNull();
        }
    }
}
