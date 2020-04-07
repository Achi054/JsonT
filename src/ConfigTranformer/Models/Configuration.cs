using System.Collections.Generic;

namespace ConfigTranformer.Models
{
    public class Configuration
    {
        private string fileName { get; set; }
        private IDictionary<string, object> sections { get; set; }

        public Configuration(string jsonFileName, IDictionary<string, object> content)
            => (fileName, sections) = (jsonFileName, content);

        public string FileName => fileName;
        public IDictionary<string, object> Sections => sections;
    }
}
