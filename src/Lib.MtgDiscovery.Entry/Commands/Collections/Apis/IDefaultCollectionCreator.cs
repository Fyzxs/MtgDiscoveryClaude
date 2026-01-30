using System.Threading.Tasks;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Apis;

public interface IDefaultCollectionCreator
{
    Task CreateDefaultCollectionAsync(string userId);
}
