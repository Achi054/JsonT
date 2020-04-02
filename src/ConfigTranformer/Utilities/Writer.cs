using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConfigTranformer.Utilities.Write
{
    /// <summary>
    /// Utility to update and write to file
    /// </summary>
    public class Writer : IWriter
    {
        /// <summary>
        /// Update the app configuration file(s) with required configuration(s)
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="appSettings"></param>
        /// <returns></returns>
        public async Task<JObject> UpdateConfigurations(IDictionary<string, object> sections, JObject appSettings)
        {
            foreach (var section in sections)
            {
                await ValidateAndCompose(appSettings, section.Key, section.Value);
            }

            return appSettings;
        }

        /// <summary>
        /// Write configuration(s) to file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task WriteConfigurationsToFile(string filePath, JObject settings)
        {
            using StreamWriter fileStream = File.CreateText(filePath);
            using JsonTextWriter writer = new JsonTextWriter(fileStream);
            await settings.WriteToAsync(writer);
        }

        private async Task ValidateAndCompose(JObject configurations, string key, object value)
        {
            var pathArray = key.Split(':');
            var pathCount = pathArray.Length;
            var propertyPath = pathCount < 2 ? pathArray[0] : string.Join('.', pathArray);

            var propertyPathValue = configurations.SelectToken(propertyPath);
            if (propertyPathValue != null && value != null)
                await UpdateValue(configurations, propertyPath, value, propertyPathValue);
            else
            {
                JProperty jProperty;
                var propertyValue = value is JProperty ? new JObject(value) : value;

                if (pathArray.Length > 1)
                {
                    jProperty = new JProperty(pathArray.LastOrDefault(), propertyValue);
                    string newKey = string.Join(':', pathArray.Take(pathArray.Length - 1));
                    await ValidateAndCompose(configurations, newKey, jProperty);
                }
                else
                {
                    jProperty = new JProperty(pathArray[0], propertyValue);
                    await UpdateAtRoot(configurations, jProperty);
                }
            }
        }

        private async Task UpdateValue(JObject configurations, string propertyPath, object value, JToken propertyValue)
        {
            switch (propertyValue.Type)
            {
                case JTokenType.Array:
                    await UpdateArrayValue(configurations, propertyPath, value);
                    break;
                case JTokenType.Object:
                    await UpdateObjectValue(configurations, propertyPath, value);
                    break;
                default:
                    await UpdateValue(configurations, propertyPath, value);
                    break;
            };
        }

        private Task UpdateObjectValue(JObject configurations, string propertyPath, object value)
        {
            var data = (JObject)configurations.SelectToken(propertyPath);

            data.AddFirst(value);

            configurations.SelectToken(propertyPath).Replace(JToken.FromObject(data));

            return Task.CompletedTask;
        }

        private Task UpdateArrayValue(JObject configurations, string propertyPath, object value)
        {
            var valueList = (JArray)value;
            var arrayList = (JArray)configurations.SelectToken(propertyPath);

            foreach (var item in valueList.Children())
            {
                arrayList.Add(item);
            }

            return Task.CompletedTask;
        }

        private Task UpdateValue(JObject configurations, string propertyPath, object value)
        {
            configurations.SelectToken(propertyPath).Replace(JToken.FromObject(value));

            return Task.CompletedTask;
        }

        private Task UpdateAtRoot(JObject configurations, object value)
        {
            configurations.AddFirst(value);

            return Task.CompletedTask;
        }
    }
}
