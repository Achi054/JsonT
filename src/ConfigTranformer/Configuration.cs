using System.Collections.Generic;

namespace ConfigTranformer
{
    public class Configuration
    {
        public string FileName { get; set; }
        public IDictionary<string, object> Sections { get; set; }
    }
}
