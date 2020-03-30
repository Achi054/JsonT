using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigTranformer.Read
{
    public interface IReader
    {
        Task<IEnumerable<Configuration>> FindAndDeserialize(string configPath);
    }
}
