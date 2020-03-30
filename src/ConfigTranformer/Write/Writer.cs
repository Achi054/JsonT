using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigTranformer.Write
{
    public class Writer : IWriter
    {
        public Task WriteToConfig(Configuration configuration, string sourcePath)
        {
            var files = Directory.GetFiles(sourcePath, configuration.FileName, SearchOption.TopDirectoryOnly);

            if (files != null && files.Any())
            {

            }

            return Task.CompletedTask;
        }
    }
}
