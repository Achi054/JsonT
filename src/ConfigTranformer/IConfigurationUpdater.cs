using System.Threading.Tasks;

namespace ConfigTranformer
{
    public interface IConfigurationUpdater
    {
        Task UpdateConfiguration(string sourcePath);
    }
}
