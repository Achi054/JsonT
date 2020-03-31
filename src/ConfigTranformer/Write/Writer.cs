using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ConfigTranformer.Write
{
    public class Writer : IWriter
    {
        public async Task WriteToConfig(Configuration configuration, string sourcePath)
        {
            var file = Directory.GetFiles(sourcePath, configuration.FileName, SearchOption.TopDirectoryOnly).FirstOrDefault();

            if (file != null)
            {
                var settings = JObject.Parse(File.ReadAllText(file));

                foreach (var section in configuration.Sections)
                {
                    await ValidateAndCompose(section.Key, section.Value, settings);
                }
            }
        }

        private async Task ValidateAndCompose(string key, object value, JObject configurations)
        {
            var pathArray = key.Split(':');
            var pathCount = pathArray.Length;
            var propertyPath = pathCount < 2 ? pathArray[0] : string.Join('.', pathArray);

            var propertyPathValue = configurations.SelectToken(propertyPath);
            if (propertyPathValue != null && value != null)
                await UpdateValue(configurations, propertyPath, value, propertyPathValue);
            else
            {
                if (pathArray.Length > 1)
                {
                    var property = new JProperty(pathArray.LastOrDefault(), value);
                    var newKey = string.Join(':', pathArray.Take(pathArray.Length - 1));
                    await ValidateAndCompose(newKey, property, configurations);
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
            }
        }

        private Task UpdateObjectValue(JObject configurations, string propertyPath, object value)
        {
            //configurations.SelectToken(propertyPath).Add(JToken.FromObject(value));

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
    }
}
