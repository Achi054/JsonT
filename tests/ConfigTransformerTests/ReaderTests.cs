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
        private readonly string configFile = @"docs\settings.json.jt";
        private readonly IReader _target;

        public ReaderTests()
        {
            configFile = AppDomain.CurrentDomain.BaseDirectory + configFile;
            _target = new Reader();
        }

        ~ReaderTests() => TestHelper.ClearContent(configFile);

        [Fact]
        public async Task FetchJsonTFiles_WhenJtFileExists_ReturnJtFile()
        {
            // Act
            var files = await _target.FetchJsonTFiles(AppDomain.CurrentDomain.BaseDirectory + "docs");

            // Assert
            files.Should().NotBeNull();
            files.Length.Should().Be(1);
        }

        [Fact]
        public async Task FetchJsonTFiles_WhenSourcePathDoesntExists_ThrowException()
        {
            // Act and Assert
            await Assert.ThrowsAsync<DirectoryNotFoundException>(async () => await _target.FetchJsonTFiles(AppDomain.CurrentDomain.BaseDirectory + @"/docs/setting.json.jt"));
        }

        [Fact]
        public async Task ReadAndDeserializeConfig_WhenConfigIsValid_ReturnJObject()
        {
            // Arrange
            var jappsettingsObject = new JObject
            {
                ["Logging:Enable"] = true,
                ["Auditing:Enable"] = true
            };

            TestHelper.AddContent(configFile, jappsettingsObject.ToString());

            // Act
            var data = await _target.ReadAndDeserializeConfig(configFile);

            // Assert
            data.Should().NotBeNull();

            data.FileName.Should().Be("settings.json");
            data.Sections.Should().NotBeNull();
            data.Sections.Should().HaveCount(2);
            data.Sections.First().Key.Should().Be("Logging:Enable");
            data.Sections.First().Value.Should().Be(true);
            data.Sections.Skip(1).First().Key.Should().Be("Auditing:Enable");
            data.Sections.Skip(1).First().Value.Should().Be(true);
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

            // Act and Assert
            await Assert.ThrowsAsync<InvalidDataException>(async () => await _target.ReadAndDeserializeConfig(configFile));
        }

        [Fact]
        public async Task ReadAndDeserializeAppConfig_WhenReadingFromConfiguration_ReturnJson()
        {
            // Arrange
            var jappsettingsObject = new JObject
            {
                ["Logging:Enable"] = true,
                ["Auditing:Enable"] = true
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
