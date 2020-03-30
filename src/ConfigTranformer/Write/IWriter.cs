using System.Threading.Tasks;

namespace ConfigTranformer.Write
{
    public interface IWriter
    {
        Task WriteToConfig(Configuration configuration, string sourcePath);
    }
}
